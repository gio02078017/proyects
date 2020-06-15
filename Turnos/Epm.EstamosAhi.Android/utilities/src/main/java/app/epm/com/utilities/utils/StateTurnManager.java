package app.epm.com.utilities.utils;

public class StateTurnManager {

    private static StateTurnManager INSTANCE = new StateTurnManager();
    private StateTurnSubject stateTurnSubject = new StateTurnSubject();


    private StateTurnManager() {
    }

    public static StateTurnManager getInstance() {
        return INSTANCE;
    }

    public StateTurnSubject getStateTurnSubject() {
        return stateTurnSubject;
    }

}
