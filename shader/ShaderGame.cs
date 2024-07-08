using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace shader;

public class ShaderGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Sprite _sprite1;
    private Effect _greyEffect, _grey2Effect, _pixelateEffect, _teleportEffect;

    private float _amount = 1;
    private float _dir = -1;

    public ShaderGame()
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

        Texture2D texture = Content.Load<Texture2D>("giraffe");
        _sprite1 = new Sprite(texture, new Vector2(100, 100));
        _greyEffect = Content.Load<Effect>("effect01");
        _grey2Effect = Content.Load<Effect>("effect02");
        _pixelateEffect = Content.Load<Effect>("pixelate");
        _teleportEffect = Content.Load<Effect>("teleport");
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _amount += (float)gameTime.ElapsedGameTime.TotalSeconds * _dir;
        if(_amount is < 0 or > 1) _dir *= -1;
        _teleportEffect.Parameters["amount"].SetValue(_amount);


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _sprite1.Draw(_spriteBatch, new Vector2(100,100));
        _spriteBatch.End();

        _spriteBatch.Begin(effect: _greyEffect);
        _sprite1.Draw(_spriteBatch, new Vector2(200,200));
        _spriteBatch.End();

        _spriteBatch.Begin(effect: _grey2Effect);
        _sprite1.Draw(_spriteBatch, new Vector2(300,100));
        _spriteBatch.End();

        _spriteBatch.Begin(effect: _pixelateEffect);
        _sprite1.Draw(_spriteBatch, new Vector2(400,200));
        _spriteBatch.End();

        _spriteBatch.Begin(effect: _teleportEffect);
        _sprite1.Draw(_spriteBatch, new Vector2(500,100));
        _spriteBatch.End();

        base.Draw(gameTime);
    }



    public class Sprite
    {
     private Texture2D _texture;
     public Vector2 Position;

     public Sprite(Texture2D texture, Vector2 position)
     {
         _texture = texture;
         Position = position;
     }

     public void Draw(SpriteBatch spriteBatch)
     {
         spriteBatch.Draw(_texture, Position, Color.White);
     }

     public void Draw(SpriteBatch spriteBatch, Vector2 position)
     {
         spriteBatch.Draw(_texture, position, Color.White);
     }
    }
}