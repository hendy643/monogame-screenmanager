using Microsoft.Xna.Framework;

namespace MoonGame.Screens
{
    public class GameScreen
    {
        public bool IsActive = true;
        public bool IsPopUp = false;
        public Color BackgroundColor = Color.CornflowerBlue;

        public virtual void LoadAssets() { }
        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
        public virtual void UnloadAssets() { }
    }
}
