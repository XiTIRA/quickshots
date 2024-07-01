using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace customcursor;

public class CustomCursorGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _spriteCursor;
    private Vector2 _spritePosition;
    private Vector2 _spriteOrigin;

    public CustomCursorGame()
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

        Texture2D hardwareCursor = Content.Load<Texture2D>("white");
        Mouse.SetCursor(
            MouseCursor.FromTexture2D(
                    hardwareCursor,
                    0,  // originx and originy should be the position on the texture that is the cursor hotspot
                    0
                )
            );

        _spriteCursor = Content.Load<Texture2D>("blue");
        // Align the sprites position to the cursor hotspot, top center, where the point is
        _spriteOrigin = new Vector2((float)_spriteCursor.Width / 2, 0);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // Get the mouse state and set the sprite position
        MouseState mouseState = Mouse.GetState();
        _spritePosition = mouseState.Position.ToVector2();

        // So we can see both, we'll drop the sprite cursor down a few pixels
        _spritePosition.Y += 50;


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(
            _spriteCursor,
            _spritePosition,
            null,
            Color.White,
            0.0f,
            _spriteOrigin,
            1.0f,
            SpriteEffects.None,
            0.0f
        );

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}