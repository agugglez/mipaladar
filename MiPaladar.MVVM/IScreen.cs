
namespace MiPaladar.MVVM
{
    public interface IScreen
    {
        bool TryToClose();

        bool IsSelfClosing();
    }
}
