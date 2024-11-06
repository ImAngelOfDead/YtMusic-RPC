namespace YTMusicRPC.utils;

public class IsDebug {
    public void DebugCheck() {
        Logger logger = Logger.Instance;
        #if DEBUG
            logger.LogInfo("Debug channel");
        #else
            logger.LogInfo("Release channel");
        #endif
    }
}