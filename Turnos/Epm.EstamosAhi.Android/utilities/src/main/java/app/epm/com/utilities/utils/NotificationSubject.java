package app.epm.com.utilities.utils;

import java.util.ArrayList;
import java.util.List;

public class NotificationSubject {
    private List<INotificationObserver> observers = new ArrayList<>();

    private List<INotificationObserver> observersCenterNotification = new ArrayList<>();

    private List<INotificationObserver.IShift> shiftStatusObservers = new ArrayList<>();


    public void attach(INotificationObserver observer) {
        observers.add(observer);
    }

    public void attachCenterNotification(INotificationObserver observer) {
        observersCenterNotification.add(observer);
    }

    public void attachShiftStatusObservers(INotificationObserver.IShift observer){
        shiftStatusObservers.add(observer);
    }

    public void addNotification(){
        notifyAllObserversNotification();
    }

    private void notifyAllObserversNotification() {
        for (INotificationObserver observer : observers) {
            observer.updateCounter();
        }
    }

    public void UpdateShiftStatus(String template){
        notifyAllShiftStatusObservers(template);
    }

    private void notifyAllShiftStatusObservers(String template){
        for (INotificationObserver.IShift observer: shiftStatusObservers){
            observer.updateState(template);
        }
    }



}
