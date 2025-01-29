using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace resizematrix;

public class ResizeMatrixGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Vector2 _preferredSize = new Vector2(1024,1024);
    private Texture2D _background;

    private Matrix _scaleMatrix;

    public ResizeMatrixGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = (int)_preferredSize.X;
        _graphics.PreferredBackBufferHeight = (int)_preferredSize.Y;
        _graphics.ApplyChanges();

        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, args) =>
        {
            GenerateScaleMatrix();
        };
        GenerateScaleMatrix();
        base.Initialize();
    }

    private void GenerateScaleMatrix()
    {
        _scaleMatrix = Matrix.CreateScale(
            _graphics.GraphicsDevice.Viewport.Width / _preferredSize.X,
            _graphics.GraphicsDevice.Viewport.Height / _preferredSize.Y,
            1f
        );
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("background");
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

        _spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.PointClamp, null, null, null, _scaleMatrix);
        _spriteBatch.Draw(_background, Vector2.Zero, Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}