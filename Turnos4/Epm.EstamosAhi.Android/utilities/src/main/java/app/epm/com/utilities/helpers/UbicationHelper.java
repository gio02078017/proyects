package app.epm.com.utilities.helpers;

import com.esri.arcgisruntime.geometry.Geometry;
import com.esri.arcgisruntime.geometry.Point;
import com.esri.arcgisruntime.geometry.PointBuilder;

/**
 * Created by josetabaresramirez on 14/02/17.
 */

public class UbicationHelper {


    public static Point getWGS84Point(Point point) {
        double y = point.getY();
        double x = point.getX();
        Point punto = new Point(point.getY(), point.getX());
        if (Math.abs(x) < 180 && Math.abs(y) < 90)
            return null;
        if ((Math.abs(x) > 20037508.3427892)
                || (Math.abs(y) > 20037508.3427892))
            return null;
        double num4 = (x / 6378137.0) * 57.295779513082323;
        double num5 = Math.floor((double) ((num4 + 180.0) / 360.0));
        double num6 = num4 - (num5 * 360.0);
        double num7 = 1.5707963267948966 - (2.0 * Math.atan(Math.exp((-1.0 * y) / 6378137.0)));
        //punto.setXY(num6, num7 * 57.295779513082323);
        return punto;
    }

    public static Point getWGS84PointBuilder(Point point) {
        double y = point.getY();
        double x = point.getX();
        //PointBuilder punto = new PointBuilder(point);
        Point beforePoint = new Point(point.getY(), point.getX());
        //PointBuilder punto = new PointBuilder(point);
        PointBuilder punto = new PointBuilder(beforePoint);
        if (Math.abs(x) < 180 && Math.abs(y) < 90)
            return null;
        if ((Math.abs(x) > 20037508.3427892)
                || (Math.abs(y) > 20037508.3427892))
            return null;
        double num4 = (x / 6378137.0) * 57.295779513082323;
        double num5 = Math.floor((double) ((num4 + 180.0) / 360.0));
        double num6 = num4 - (num5 * 360.0);
        double num7 = 1.5707963267948966 - (2.0 * Math.atan(Math.exp((-1.0 * y) / 6378137.0)));
        punto.setXY(num6, num7 * 57.295779513082323);
        return punto.toGeometry();
    }

    public static Point fromJsonToPoint(String point){
        Geometry pointSelected =  Point.fromJson(point);
        return (Point)pointSelected;
    }

    public static Point fromJsonToPointAndInverse(String point){
        Geometry pointSelected =  Point.fromJson(point);
        Point currentPoint = (Point)pointSelected;
        return new Point(currentPoint.getY(), currentPoint.getX());
    }
}
