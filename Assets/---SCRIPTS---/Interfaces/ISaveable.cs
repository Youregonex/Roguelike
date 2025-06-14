
namespace Yg.SaveLoad
{
    public interface ISaveable
    {
        public object CaptureState();
        public void RestoreState(object data);
    }
}
