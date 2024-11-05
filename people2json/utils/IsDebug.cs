namespace YTMusicRPC.utils;

public class IsDebug {
    public void DebugCheck() {
        Logger logger = new Logger();
        #if DEBUG
            logger.LogInfo("Debug channel");
        #else
            logger.LogInfo("Release channel");
        #endif
    }
}