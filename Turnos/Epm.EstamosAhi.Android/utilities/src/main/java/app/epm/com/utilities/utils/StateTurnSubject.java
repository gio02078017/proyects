package app.epm.com.utilities.utils;

import java.util.ArrayList;
import java.util.List;

public class StateTurnSubject {

    private List<IStateTurnObserver> observers = new ArrayList<>();
    private List<IStateTurnObserver> observersNotification = new ArrayList<>();

    public void attach(IStateTurnObserver observer) {
        observers.add(observer);
    }

    public void changeState(){
        notifyAllObserversNotification();
    }

    private void notifyAllObserversNotification() {
        for (IStateTurnObserver observer : observers) {
            observer.changeState();
        }
    }

    public void removeSubscription(IStateTurnObserver observer){
        observers.remove(observer);
    }

}
