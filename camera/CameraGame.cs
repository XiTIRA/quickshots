using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace camera;

public class CameraGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _background;
    private Vector2 _position;
    private FollowCamera _followCamera;
    private Vector2 _screenSize;

    public CameraGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        _followCamera = new(Vector2.Zero);
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("background");
        _screenSize = new(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        KeyboardState kstate = Keyboard.GetState();

        if (kstate.IsKeyDown(Keys.W)) _position.Y -= 50;
        if (kstate.IsKeyDown(Keys.A)) _position.X -= 50;
        if (kstate.IsKeyDown(Keys.S)) _position.Y += 50;
        if (kstate.IsKeyDown(Keys.D)) _position.X += 50;

        _followCamera.Follow(new Rectangle((int)_position.X,(int)_position.Y,50,50), _screenSize);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);

        _spriteBatch.End();

        base.Draw(gameTime);
    }
}

public class FollowCamera
{
    public Vector2 Position;

    public FollowCamera(Vector2 position)
    {
        Position = position;
    }

    public void Follow(Rectangle rectangle, Vector2 screenSize)
    {
        Position = new Vector2(
            -rectangle.X + (screenSize.X / 2 - rectangle.Width / 2),
            -rectangle.Y + (screenSize.Y / 2 - rectangle.Height / 2)
        );
    }
}