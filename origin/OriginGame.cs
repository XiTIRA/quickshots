using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace origin;

public class OriginGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _bar;
    private Texture2D _box;
    private SpriteFont _font;

    private Vector2 _screenSize;
    
    private Vector2 _origin = Vector2.Zero;
    private Origin _originType = Origin.TopLeft;

    private KeyboardState _prevKeyboardState;
    
    public OriginGame()
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

        _bar = Content.Load<Texture2D>("bar");
        _box = Content.Load<Texture2D>("metalPanel");
        _font = Content.Load<SpriteFont>("font");
        
        _screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
        _prevKeyboardState = Keyboard.GetState();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space) && !_prevKeyboardState.IsKeyDown(Keys.Space))
            _originType = _originType - 1 < 0 ? Origin.CenterBottom : _originType - 1;

        _origin = GetOrigin(_box, _originType);
        
        _prevKeyboardState = keyboardState;
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(
            _box,
            _screenSize / 2,
            null,
            Color.White,
            0.0f,
            _origin,
            1.0f,
            SpriteEffects.None,
            0.0f
            );
        
        _spriteBatch.Draw(
            _bar, 
            new Rectangle(0,(int)(_screenSize.Y/2)-2,(int)_screenSize.X,_bar.Height),
            Color.White);
        
        _spriteBatch.Draw(
            _bar,
            new Rectangle((int)(_screenSize.X/2)-2,0,3, (int)_screenSize.Y),
            Color.White
            );
        
        _spriteBatch.DrawString(_font, $"Origin: {_origin}", new Vector2(10,10), Color.White);
        _spriteBatch.DrawString(_font, $"Position: {_screenSize/2}", new Vector2(10,40), Color.White);
        _spriteBatch.DrawString(_font, $"Start Draw From: {_originType}", new Vector2(10,70), Color.White);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    private enum Origin
    {
        Center,
        TopLeft,
        TopRight,
        BottomLeft,
        BottomRight,
        CenterLeft,
        CenterRight,
        CenterTop,
        CenterBottom
    }

    private Vector2 GetOrigin(Texture2D texture, Origin origin)
    {
        return origin switch
        {
            Origin.Center => new Vector2((int)(texture.Width / 2), (int)(texture.Height / 2)),
            Origin.TopLeft => Vector2.Zero,
            Origin.TopRight => new Vector2(texture.Width, 0),
            Origin.BottomLeft => new Vector2(0, texture.Height),
            Origin.BottomRight => new Vector2(texture.Width, texture.Height),
            Origin.CenterLeft => new Vector2(0, (int)(texture.Height / 2)),
            Origin.CenterRight => new Vector2(texture.Width, (int)(texture.Height / 2)),
            Origin.CenterTop => new Vector2((int)(texture.Width / 2), 0),
            Origin.CenterBottom => new Vector2((int)(texture.Width / 2), texture.Height),
            _ => Vector2.Zero
        };
    }
    
}