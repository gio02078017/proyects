package app.epm.com.utilities.utils;

import java.util.ArrayList;
import java.util.List;

public class ChangeStateTurnSubject {

    private List<IChangeStateTurnObserver> observers = new ArrayList<>();
    private List<IChangeStateTurnObserver> observersNotification = new ArrayList<>();

    public void attach(IChangeStateTurnObserver observer) {
        observers.add(observer);
    }

    public void changeState(){
        notifyAllObserversNotification();
    }

    private void notifyAllObserversNotification() {
        for (IChangeStateTurnObserver observer : observers) {
            observer.updateTurn();
        }
    }

    public void removeSubscription(IChangeStateTurnObserver observer){
        observers.remove(observer);
    }

}
