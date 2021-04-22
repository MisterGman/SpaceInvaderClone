using System.Collections;
namespace _Game.Scripts
{
    public interface IBulletCooldown
    {
        IEnumerator CooldownUntilBulletDestroyed();
    }
}
