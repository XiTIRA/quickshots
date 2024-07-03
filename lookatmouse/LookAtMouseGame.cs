using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lookatmouse;

public class LookAtMouseGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _greenArrow;
    private Vector2 _greenArrowOrigin;
    private Vector2 _greenArrowPosition;
    private float _greenArrowRotation = 0f;
    
    private Texture2D _blueArrow;
    private Vector2 _blueArrowOrigin;
    private Vector2 _blueArrowPosition;
    private float _blueArrowRotation = 0f;
    
    public LookAtMouseGame()
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

        _greenArrow = Content.Load<Texture2D>("greenArrow");
        _greenArrowOrigin = new Vector2((float)_greenArrow.Width / 2,(float) _greenArrow.Height /2);
        _greenArrowPosition = new Vector2(GraphicsDevice.Viewport.Width * .25f, GraphicsDevice.Viewport.Height/2f);
        
        _blueArrow = Content.Load<Texture2D>("blueArrow");
        _blueArrowOrigin = new Vector2((float)_greenArrow.Width / 2,(float) _greenArrow.Height /2);
        _blueArrowPosition = new Vector2(GraphicsDevice.Viewport.Width * .75f, GraphicsDevice.Viewport.Height/2f);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Mouse.GetState();
        Vector2 mousePosition = mouseState.Position.ToVector2();
        
        Vector2 mouseToGreen = mousePosition - _greenArrowPosition;
        _greenArrowRotation = (float)Math.Atan2(mouseToGreen.Y,mouseToGreen.X);

        Vector2 mouseToBlue = mousePosition - _blueArrowPosition;
        _blueArrowRotation = (float)Math.Atan2(mouseToBlue.Y,mouseToBlue.X);
        _blueArrowRotation += MathHelper.ToRadians(180);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(
            _greenArrow,
            _greenArrowPosition,
            null,
            Color.White,
            _greenArrowRotation,
            _greenArrowOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );
        
        _spriteBatch.Draw(
            _blueArrow,
            _blueArrowPosition,
            null,
            Color.White,
            _blueArrowRotation,
            _blueArrowOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );
        
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }
}