package app.epm.com.utilities.helpers;

import android.content.Context;
import android.content.res.XmlResourceParser;
import android.util.Log;

import org.xmlpull.v1.XmlPullParserException;
import java.io.IOException;
import java.util.ArrayList;

import app.epm.com.utilities.entities.FeaturesPointMap;
import app.epm.com.utilities.entities.FeaturesPointMapItem;
import app.epm.com.utilities.utils.Constants;

public class ProcessXMLData {

    final static String TAP = "Error_ProcessXMLData";

    private static XmlResourceParser parserXml(Context context, String xmlName){

        int xmlId = context.getResources().getIdentifier(xmlName,
                "xml", context.getPackageName());

        XmlResourceParser parser = context.getResources().getXml(xmlId);

        return parser;

    }

    public static FeaturesPointMap convertXMLToObject(Context context, String xmlName){
        try{
            XmlResourceParser parser = parserXml(context, xmlName);
            ArrayList<FeaturesPointMapItem> listFeaturesPointMapItem = process(parser);
            FeaturesPointMap featuresPointMap = new FeaturesPointMap();
            featuresPointMap.setListFeaturesPointMapItem(listFeaturesPointMapItem);
            return listFeaturesPointMapItem == null || listFeaturesPointMapItem.size() == 0 ? null : featuresPointMap;
        }catch(IOException e){
            Log.e(TAP,e.getMessage());
            return null;
        }catch (XmlPullParserException e){
            Log.e(TAP,e.getMessage());
            return null;
        }
    }

    private static ArrayList<FeaturesPointMapItem> process(XmlResourceParser parser)throws IOException,XmlPullParserException {
        ArrayList<FeaturesPointMapItem> listFeaturesPointMapItem = new ArrayList<FeaturesPointMapItem>();
        int eventType = -1;
        while (eventType != parser.END_DOCUMENT) {
            if (eventType == XmlResourceParser.START_TAG) {
                String itemValue = parser.getName();
                if (itemValue.equalsIgnoreCase(Constants.ITEM_FEATURE_POINT_MAP)) {
                    listFeaturesPointMapItem.add(new FeaturesPointMapItem(parser.getAttributeValue(null, Constants.ITEM_NAME_FEATURE_POINT_MAP),parser.getAttributeValue(null, Constants.ITEM_SEARCH_FEATURE_POINT_MAP),parser.getAttributeValue(null, Constants.REQUIRE_SEARCH_FEATURE_POINT_MAP),parser.getAttributeValue(null, Constants.EMPTY_SEARCH_FEATURE_POINT_MAP)));
                }

            }
            eventType = parser.next();
        }

        return listFeaturesPointMapItem;
    }

}
