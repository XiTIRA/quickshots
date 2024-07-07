using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace font;

public class FontGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private SpriteFont _arial;
    private SpriteFont _chancery;
    private SpriteFont _future;

    private int _centerX;

    public FontGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _arial = Content.Load<SpriteFont>("arial");

        _chancery = Content.Load<SpriteFont>("chancery");
        _future = Content.Load<SpriteFont>("future");

        _centerX = _graphics.PreferredBackBufferWidth / 2;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.DrawString(_arial, "Hello, World! Arial System Font", new Vector2(100, 100), Color.White);
        _spriteBatch.DrawString(_chancery, "Hello, World! Chancery Custom Font", new Vector2(100, 200), Color.White);
        _spriteBatch.DrawString(_future, "Hello, World! Kenny Custom Font", new Vector2(100, 300), Color.White);

        _spriteBatch.DrawString(_future, "Kenny Centered", new Vector2(_centerX, 20), Color.White, 0, new Vector2(_future.MeasureString("Kenny Centered").X / 2, 0), 1, SpriteEffects.None, 0);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}