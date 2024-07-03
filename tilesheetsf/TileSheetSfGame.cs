using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace tilesheetsf;

public class TileSheetSfGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private List<Rectangle> _tileCollisions;
    private Tilemap _tilemap;
    public TileSheetSfGame()
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

        var tileMapJson = File.ReadAllText("Content/map.json") ;
        Texture2D tileMapTexture = Content.Load<Texture2D>("spritesheet");
        Atlas tileMapAtlas = new Atlas(tileMapTexture, new Vector2(6,8));
        _tilemap = new Tilemap(tileMapAtlas, Vector2.Zero, false);
        _tilemap.AddSpriteFusionTiles(tileMapJson);
        _tileCollisions = _tilemap.GetCollisionRectangles();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
            Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _tilemap.Draw(_spriteBatch);
        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    public class Tilemap
    {
        private Vector2 _startPosition;
        private Atlas _atlas;
        private Vector2 _tileSize;
        private List<TileLayer> _tileLayers;
        private bool _showColliders;

        public Tilemap(Atlas atlas, Vector2 startPosition, bool showColliders)
        {
            _atlas = atlas;
            _startPosition = startPosition;
            _tileSize = atlas.GetFrameSize();
            _tileLayers = new List<TileLayer>();
            _showColliders = showColliders;
        }

        public List<Rectangle> GetCollisionRectangles()
        {
            List<Rectangle> rectangles = new List<Rectangle>();

            foreach (var layer in _tileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.IsCollidable)
                    {
                        rectangles.Add(new Rectangle(
                            (int)((tile.GridPosition.X * _tileSize.X) + _startPosition.X),
                            (int)((tile.GridPosition.Y * _tileSize.Y) + _startPosition.Y),
                            (int)_tileSize.X,
                            (int)_tileSize.Y));
                    }
                }
            }

            return rectangles;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var layer in _tileLayers)
            {
                foreach (var tile in layer.Tiles)
                {
                    if (tile.IsCollidable && !_showColliders)
                        continue;
                    _atlas.Draw(spriteBatch, tile.TileId, (tile.GridPosition * _tileSize) + _startPosition, false, 0.0f,
                        Vector2.Zero);
                }
            }
        }


        public void AddSpriteFusionTiles(string json)
        {
            SpriteFusionMap map = JsonSerializer.Deserialize<SpriteFusionMap>(json);
            foreach (var layer in map.layers)
            {
                TileLayer tileLayer = new TileLayer();

                foreach (var tile in layer.tiles)
                {
                    int tileId = int.Parse(tile.id);
                    tileLayer.AddTile(tileId, new Vector2(tile.x, tile.y), layer.collider);
                }

                _tileLayers.Add(tileLayer);
            }

            _tileLayers.Reverse();
        }

        private class SpriteFusionMap
        {
            public int tileSize { get; set; }
            public int mapWidth { get; set; }
            public int mapHeight { get; set; }
            public List<SpriteFusionLayer> layers { get; set; }
        }

        private class SpriteFusionLayer
        {
            public string name { get; set; }
            public List<SpriteFusionTile> tiles { get; set; }
            public bool collider { get; set; }
        }

        private class SpriteFusionTile
        {
            public string id { get; set; }
            public int x { get; set; }
            public int y { get; set; }
        }
    }

    public class TileLayer
        {
            public TileLayer()
            {
                Tiles = new List<Tile>();
            }

            public List<Tile> Tiles { get; set; }

            public void AddTile(int tileId, Vector2 gridPosition, bool isCollidable)
            {
                Tiles.Add(new Tile { TileId = tileId, GridPosition = gridPosition, IsCollidable = isCollidable });
            }
        }

        public struct Tile
        {
            public int TileId { get; set; }
            public Vector2 GridPosition { get; set; }
            public bool IsCollidable { get; set; }
        }
    
        public class Atlas
{
    private Texture2D _texture;
    private int _tileWidth;
    private int _tileHeight;
    private Vector2 _gridSize;
    private Dictionary<int,Rectangle> _frames;

    /// <summary>
    /// Handles reading a texture as an atlas.
    /// </summary>
    /// <param name="texture">Texture to load as an atlas.</param>
    /// <param name="gridSize">Vector2 representing the number of columns and rows in this tileset.</param>
    public Atlas(Texture2D texture, Vector2 gridSize)
    {
        _texture = texture;
        _tileWidth = _texture.Width / (int) gridSize.Y; // Divide width by the columns
        _tileHeight = _texture.Height / (int) gridSize.X; // Divide height by the rows
        _gridSize = gridSize;

        _frames = new Dictionary<int,Rectangle>();

        int frame = 0;
        for (int x = 0; x < gridSize.X; x++)
        {
            for (int y = 0; y < gridSize.Y; y++)
            {
                // Swap X and Y, remembering x is the number of rows and y is the number of columns
                // Ie, 10 Rows (grid.x) would split vertically (Texture.Y)
                _frames.Add(frame, new Rectangle(y * _tileWidth, x * _tileHeight, _tileWidth, _tileHeight));
                frame++;
            }
        }
    }

    public Vector2 GetFrameSize()
    {
        return new Vector2(_tileWidth, _tileHeight);
    }

    /// <summary>
    /// Draws respective frame from the atlas to the spritebatch.
    /// </summary>
    /// <param name="spriteBatch"></param>
    /// <param name="frame">frame number ot use.</param>
    /// <param name="position">X and Y position.</param>
    public void Draw(SpriteBatch spriteBatch, int frame, Vector2 position, bool flipX, float rotation, Vector2 origin)
    {
        spriteBatch.Draw(_texture, position, _frames[frame], Color.White, rotation, origin, 1, flipX ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0.0f);
    }


}
}