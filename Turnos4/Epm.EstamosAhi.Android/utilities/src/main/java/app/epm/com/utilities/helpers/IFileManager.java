package app.epm.com.utilities.helpers;

import android.content.Context;
import android.graphics.Bitmap;
import android.net.Uri;
import android.widget.ImageView;

import java.io.File;
import java.io.IOException;
import java.io.InputStream;
import java.util.ArrayList;

/**
 * Created by leidycarolinazuluagabastidas on 21/03/17.
 */

public interface IFileManager {

    File createImageFile() throws IOException;

    String createPathForAudio();

    File saveImage(Bitmap bitmap, String fileName) throws IOException;

    String getSizeFile(String path);

    String getExtensionFile(String path);

    String getContentTypeFile(String type);

    String fileToBytesBase64(String attach) throws Exception;

    byte[] getByteArrayFromInputStream(InputStream inputStream) throws IOException;

    String getRealPath(String path);

    boolean verifyMaxMegaBytesArrayFiles(int valueMax, ArrayList<String> arrayFiles);

    ImageView getImageViewInstance();

    byte[] fileToBytes(File file);
}
