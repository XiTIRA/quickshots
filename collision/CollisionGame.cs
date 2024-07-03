using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace collision;

public class CollisionGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private Texture2D _bar;

    private Sprite _staticSquare;
    private Sprite _movingSquare;
    private Sprite _staticCircle;
    private Sprite _movingCircle;

    public CollisionGame()
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
        
        Vector2 screenSize = new Vector2(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);

        var redRound = Content.Load<Texture2D>("redRound");
        var greenRound = Content.Load<Texture2D>("greenRound");
        var redSquare = Content.Load<Texture2D>("redSquare");
        var greenSquare = Content.Load<Texture2D>("greenSquare");
        _bar = Content.Load<Texture2D>("bar");
        
        _staticSquare = new Sprite(new Vector2(screenSize.X*.25f, screenSize.Y * 0.25f), redSquare, greenSquare);        
        _staticCircle = new Sprite(new Vector2(screenSize.X*.75f, screenSize.Y * 0.75f), redRound, greenRound);

        _movingSquare = new Sprite(new Vector2(0, screenSize.Y * 0.25f), redSquare, greenSquare);
        _movingCircle = new Sprite(new Vector2(0, screenSize.Y * 0.75f), redRound, greenRound);
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        MouseState mouseState = Mouse.GetState();

        _movingSquare.Position.X = mouseState.X;
        _movingCircle.Position.X = mouseState.X;
        
        if (_staticSquare.Rectangle.Intersects(_movingSquare.Rectangle) )
        {
            _staticSquare.Collided = true;
            _movingSquare.Collided = true;
        }
        else
        {
            _staticSquare.Collided = false;
            _movingSquare.Collided = false;
        }

        if (Vector2.Distance(_movingCircle.Position, _staticCircle.Position) < _movingCircle.Radius + _staticCircle.Radius)
        {
            _movingCircle.Collided = true;
            _staticCircle.Collided = true;
        }        
        else
        {
            _movingCircle.Collided = false;
            _staticCircle.Collided = false;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        var bound = _staticSquare.Rectangle;
        var pos = _staticSquare.Position;
        var noOrigin = new Rectangle((int)( pos.X),(int)( pos.Y), bound.Width, bound.Height);

        _spriteBatch.Begin();
        
        _staticSquare.Draw(_spriteBatch);
        _movingSquare.Draw(_spriteBatch);
        _staticCircle.Draw(_spriteBatch);
        _movingCircle.Draw(_spriteBatch);

        _spriteBatch.Draw(_bar, new Rectangle(bound.X,bound.Y,bound.Width,3), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(bound.X,bound.Y+bound.Height,bound.Width,3), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(bound.X,bound.Y,3,bound.Height), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(bound.X+bound.Width,bound.Y,3,bound.Height), Color.White);

        _spriteBatch.Draw(_bar, new Rectangle(noOrigin.X,noOrigin.Y,noOrigin.Width,1), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(noOrigin.X,noOrigin.Y+noOrigin.Height,noOrigin.Width,1), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(noOrigin.X,noOrigin.Y,1,noOrigin.Height), Color.White);
        _spriteBatch.Draw(_bar, new Rectangle(noOrigin.X+noOrigin.Width,noOrigin.Y,1,noOrigin.Height), Color.White);


        _spriteBatch.End();

        base.Draw(gameTime);
    }
    
    
    // *****************************

    public class Sprite
    {
        public Vector2 Position;
        public Texture2D RedTexture { get; set; }
        public Texture2D GreenTexture { get; set; }
        public bool Collided;
        public Vector2 Orign;
        
        public Rectangle Rectangle => new((int)(Position.X - Orign.X), (int)(Position.Y - Orign.Y), RedTexture.Width, RedTexture.Height);
        public float Radius => Rectangle.Width / 2f;
        
        public Sprite (Vector2 position, Texture2D red, Texture2D green)
        {
            Position = position;
            RedTexture = red;
            GreenTexture = green;
            Orign = new Vector2(RedTexture.Width / 2f, RedTexture.Height / 2f);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var texture =  Collided ? RedTexture : GreenTexture;
            spriteBatch.Draw(
                texture, 
                Position, 
                null, 
                Color.White, 
                0, 
                Orign, 
                1,
                SpriteEffects.None, 
                0
                );
        }
    }
}