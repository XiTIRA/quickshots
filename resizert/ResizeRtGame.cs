using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace resizert;

public class ResizeRtGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private RenderTarget2D _finalTarget;
    private Rectangle _finalDestination;
    private bool _isResizing;

    private Texture2D _background;

    private Vector2 _preferredSize = new Vector2(1024,1024);    public ResizeRtGame()

    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        Window.AllowUserResizing = true;
        Window.ClientSizeChanged += (sender, args) =>
        {
            if (!_isResizing && Window.ClientBounds.Width > 0 && Window.ClientBounds.Height > 0)
            {
                _isResizing = true;
                CalculateRenderDestination();
                _isResizing = false;
            }
        };

        base.Initialize();
    }


    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        _finalTarget = new RenderTarget2D(GraphicsDevice, (int)_preferredSize.X, (int)_preferredSize.Y);
        CalculateRenderDestination(); // Needs to be called at least once to set the final destination rect
        _background = Content.Load<Texture2D>("Background");
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

        GraphicsDevice.SetRenderTarget(_finalTarget);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_background, new Rectangle(0, 0, _finalTarget.Width, _finalTarget.Height), color: Color.White);

        _spriteBatch.End();

        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_finalTarget, _finalDestination, color: Color.White);
        _spriteBatch.End();

        base.Draw(gameTime);

        base.Draw(gameTime);
    }



    private void CalculateRenderDestination()
    {
        Point size = GraphicsDevice.Viewport.Bounds.Size;
        float scalex = (float)size.X / _finalTarget.Width;
        float scaley = (float)size.Y / _finalTarget.Height;

        float scale = Math.Min(scaley, scalex);

        _finalDestination.Width = (int)(_finalTarget.Width * scale);
        _finalDestination.Height = (int)(_finalTarget.Height * scale);

        _finalDestination.X = (size.X - _finalDestination.Width) / 2;
        _finalDestination.Y = (size.Y - _finalDestination.Height) / 2;
    }
}