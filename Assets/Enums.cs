using System.ComponentModel;
namespace Scripts
{
    enum SceneState
    {
        [Description("INSPECT")]
        Inspect,
        [Description("MODIFY")]
        Modify
    }

    enum AnimationState
    {
        Changing,
        Finished
    }
}