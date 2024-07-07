using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace platformplayer;

public class PlatformPlayerGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player _player;
    private Texture2D _background;

    public PlatformPlayerGame()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        _graphics.PreferredBackBufferWidth = 1024;
        _graphics.PreferredBackBufferHeight = 1024;
        _graphics.ApplyChanges();

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _background = Content.Load<Texture2D>("background");
        _player = new Player(Content.Load<Texture2D>("giraffe"), new Vector2(200,200));
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        _player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        _spriteBatch.Draw(_background, new Vector2(0, 0), Color.White);
        _player.Draw(_spriteBatch);

        _spriteBatch.End();

        base.Draw(gameTime);
    }


    public class Player
    {
        private Texture2D _texture;
        private Vector2 _velocity = Vector2.Zero;

        private float _groundFriction = 0.4f;
        private float _gravity = 0.5f;
        private float _gravityMultiplier = 2.75f;
        private float _maxGravity = 30f;

        private float _jumpForce = 2;
        private float _maxJumpForce = 10;
        private float _maxSpeed = 5f;
        private float _accelleration = .35f;
        private float _pixelPerSecond = 200f;

        private bool _isGrounded = true;
        private bool _continueJump = true;

        private Vector2 _position;

        public Player(Texture2D texture, Vector2 initialPosition)
        {
            _texture = texture;
            _position = initialPosition;
        }

        public void Update(GameTime gameTime)
        {
            bool left = Keyboard.GetState().IsKeyDown(Keys.A);
            bool right = Keyboard.GetState().IsKeyDown(Keys.D);
            bool jump = Keyboard.GetState().IsKeyDown(Keys.Space);

            if (jump )
            {
                if (_isGrounded)
                {
                    _velocity.Y = -_jumpForce;
                    _isGrounded = false;
                    _continueJump = true;
                }
                else if (_continueJump)
                {
                    _velocity.Y -= _jumpForce;
                    _velocity.Y = MathHelper.Clamp(_velocity.Y, -_maxJumpForce, _maxGravity);

                    _continueJump = !(_velocity.Y == -_maxJumpForce);
                }
            }
            else
            {
                _continueJump = false;
            }

            if (left)
            {
                _velocity.X += -_accelleration;
            }
            else if (right)
            {
                _velocity.X += _accelleration;
            }
            else
            {
                if (_velocity.X < 0)
                {
                    _velocity.X += _groundFriction;
                    if(_velocity.X > 0) _velocity.X = 0;
                }
                else if (_velocity.X > 0)
                {
                    _velocity.X -= _groundFriction;
                    if(_velocity.X < 0) _velocity.X = 0;
                }

            }


            _velocity.Y += _velocity.Y < 0 ? _gravity : _gravity * _gravityMultiplier;
            _velocity.Y = MathHelper.Clamp(_velocity.Y, -_maxJumpForce, _maxGravity);


            _velocity.X = MathHelper.Clamp(_velocity.X, -_maxSpeed, _maxSpeed);

            _position += _velocity * (float)gameTime.ElapsedGameTime.TotalSeconds * _pixelPerSecond;

            _position.Y = MathHelper.Clamp(_position.Y, 0, 800);

            _isGrounded = _position.Y == 800f;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _position, Color.White);
        }
    }
}