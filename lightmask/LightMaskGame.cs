using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace lightmask;

public class LightMaskGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _background;
    private Texture2D _light;
    private RenderTarget2D _rtLight;
    
    public LightMaskGame()
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

        _rtLight = new RenderTarget2D(GraphicsDevice, 800, 600);
        _light = Content.Load<Texture2D>("window");
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
        var blend = new BlendState
        {
            AlphaBlendFunction = BlendFunction.ReverseSubtract,
            AlphaSourceBlend = Blend.One,
            AlphaDestinationBlend = Blend.One,
        };
        
        GraphicsDevice.SetRenderTarget(_rtLight);
        GraphicsDevice.Clear(new Color(0,0,0,220));
        
        _spriteBatch.Begin(blendState: blend);
        _spriteBatch.Draw(_light, new Rectangle(0, 0, 800, 600), Color.White);
        _spriteBatch.End();
        
        GraphicsDevice.SetRenderTarget(null);
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        
        _spriteBatch.Draw(_background, new Rectangle(0, 0, 800, 600), Color.White);
        _spriteBatch.Draw(_rtLight, new Rectangle(0, 0, 800, 600), Color.White);
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}