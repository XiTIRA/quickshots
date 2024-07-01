using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace buttons;

public class ButtonsGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SpriteFont _spriteFont;
    private SimpleButton _simpleButton;
    private Button _button;

    public int _clickCount1 = 0;
    private int _clickCount2 = 0;

    public ButtonsGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        int width = _graphics.PreferredBackBufferWidth;
        int height = _graphics.PreferredBackBufferHeight;

        Texture2D buttonBase = Content.Load<Texture2D>("base");
        Texture2D buttonClick = Content.Load<Texture2D>("click");
        Texture2D buttonHover = Content.Load<Texture2D>("hover");

        _spriteFont = Content.Load<SpriteFont>("font");

        _simpleButton = new SimpleButton(
            buttonBase,
            new Vector2(100,(float)height/2),
            Color.LightGray,
            Color.Gray,
            _spriteFont,
            "Button 1");

        _simpleButton._onClick += Increment1;

        _button = new Button(
            buttonBase,
            new Vector2(width-100-buttonBase.Width,(float)height/2),
            buttonHover,
            buttonClick,
            _spriteFont,
            "Button 2");

        _button._onClick += Increment2;
    }

    private void Increment1()
    {
        _clickCount1++;
    }

    private void Increment2()
    {
        _clickCount2++;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Mouse.GetState();
        Vector2 mousePos = mouseState.Position.ToVector2();
        bool mouseClicked = mouseState.LeftButton == ButtonState.Pressed;

        _simpleButton.Update(gameTime, mousePos, mouseClicked);
        _button.Update(gameTime, mousePos, mouseClicked);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _simpleButton.Draw(_spriteBatch, gameTime);
        _button.Draw(_spriteBatch, gameTime);

        _spriteBatch.DrawString(_spriteFont,$"{_clickCount1}",new Vector2(200,200), Color.White);
        _spriteBatch.DrawString(_spriteFont,$"{_clickCount2}",new Vector2(550,200), Color.White);


        _spriteBatch.End();

        base.Draw(gameTime);
    }


    public class SimpleButton
    {
        private Vector2 _position;
        private Texture2D _texture;
        private SpriteFont _spriteFont;
        private Color _hoverTint;
        private Color _clickTint;
        private Color _activeTint = Color.White;
        private Vector2 _origin = Vector2.Zero;

        public event Action _onClick;

        private string _text;
        private Vector2 _textOffset;

        private bool _mouseWasDown = false;

        public SimpleButton(Texture2D texture, Vector2 position, Color hoverTint, Color clickTint, SpriteFont spriteFont, string text)
        {
            _spriteFont = spriteFont;
            _texture = texture;
            _position = position;

            _hoverTint = hoverTint;
            _clickTint = clickTint;

            _text = text;
            _textOffset = new Vector2(20, 20);
        }

        public void Update(GameTime gameTime, Vector2 mousePos, bool mouseDown)
        {
            Rectangle buttonBounds = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            bool isHovering = buttonBounds.Contains((int)mousePos.X, (int)mousePos.Y);

            if (isHovering)
            {
                if (mouseDown)
                {
                    _activeTint = _clickTint;
                    if(!_mouseWasDown)
                        _onClick?.Invoke();
                    _mouseWasDown = true;
                }
                else
                {
                    _activeTint = _hoverTint;
                    _mouseWasDown = false;
                }
            }
            else
            {
                _activeTint = Color.White;
                _mouseWasDown = false;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                _texture,
                _position,
                null,
                _activeTint,
                0.0f,
                _origin,
                1.0f,
                SpriteEffects.None,
                0.0f
            );
            spriteBatch.DrawString(_spriteFont,_text,_position+_textOffset, Color.White);
        }
    }

    public class Button
    {
        private Vector2 _position;
        private Texture2D _texture;
        private SpriteFont _spriteFont;
        private Texture2D _hoverTexture;
        private Texture2D _clickTexture;
        private Texture2D _activeTexture;
        private Vector2 _origin = Vector2.Zero;

        private string _text;
        private Vector2 _textOffset;

        public event Action _onClick;

        public Button(Texture2D texture, Vector2 position, Texture2D hoverTexture, Texture2D clickTexture,
            SpriteFont spriteFont, string text)
        {
            _spriteFont = spriteFont;
            _texture = texture;
            _position = position;

            _hoverTexture = hoverTexture;
            _clickTexture = clickTexture;

            _text = text;
            _textOffset = new Vector2(20, 20);
        }

        public void Update(GameTime gameTime, Vector2 mousePos, bool mouseDown)
        {
            Rectangle buttonBounds = new Rectangle((int)_position.X, (int)_position.Y, _texture.Width, _texture.Height);
            bool isHovering = buttonBounds.Contains((int)mousePos.X, (int)mousePos.Y);

            if (isHovering)
            {
                if (mouseDown)
                {
                    _activeTexture = _clickTexture;
                    _onClick?.Invoke();
                }
                else
                {
                    _activeTexture = _hoverTexture;
                }
            }
            else
            {
                _activeTexture = _texture;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(
                _activeTexture,
                _position,
                null,
                Color.White,
                0.0f,
                _origin,
                1.0f,
                SpriteEffects.None,
                0.0f
            );
            spriteBatch.DrawString(_spriteFont,_text,_position+_textOffset, Color.White);
        }
    }
}