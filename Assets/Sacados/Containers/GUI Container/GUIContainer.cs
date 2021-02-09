// Only for the server (even if the file do not have the suffix "Server")
// Because if the server have no class named GUIContainer then it's not going to work

#if IS_SERVER

namespace Sacados.Containers {

    public abstract partial class GUIContainer : Container { }

}

#endif