package app.epm.com.utilities.helpers;

import android.content.ContentResolver;
import android.content.Context;
import android.database.Cursor;
import android.graphics.Bitmap;
import android.net.Uri;
import android.os.Environment;
import android.provider.MediaStore;
import android.util.Base64;
import android.util.Log;
import android.webkit.MimeTypeMap;
import android.widget.ImageView;
import android.widget.LinearLayout;

import java.io.ByteArrayOutputStream;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.text.DecimalFormat;
import java.text.SimpleDateFormat;
import java.util.ArrayList;
import java.util.Date;

import app.epm.com.utilities.controls.SquareImageView;
import app.epm.com.utilities.utils.Constants;

public class FileManager implements IFileManager {

    private final Context context;
    private String TAG = this.getClass().getName();

    public FileManager(Context context) {
        this.context = context;
    }

    @Override
    public File createImageFile() throws IOException {
        String imageFileName = Constants.PREFIX_FILE_IMAGE + new SimpleDateFormat(Constants.FORMAT_DATE_FILE).format(new Date());
        File storageDir = context.getExternalFilesDir(Environment.DIRECTORY_PICTURES);
        if (!storageDir.exists()) {
            boolean result = storageDir.mkdir();
            if (!result) {
                return null;
            }
        }

        return File.createTempFile(
                imageFileName,  /* prefix */
                Constants.SUFFIX_FILE_IMAGE, /* suffix */
                storageDir      /* directory */
        );
    }

    @Override
    public String createPathForAudio() {
        String timeStamp = new SimpleDateFormat(Constants.FORMAT_DATE_FILE).format(new Date());
        String storageDir = Environment.getExternalStorageDirectory().getAbsolutePath();
        return storageDir + "/" + Constants.PREFIX_FILE_AUDIO + timeStamp + Constants.SUFFIX_FILE_AUDIO;
    }

    @Override
    public File saveImage(Bitmap bitMap, String fileName) throws IOException {
        if (bitMap != null) {
            ByteArrayOutputStream stream = new ByteArrayOutputStream();

            bitMap.compress(Bitmap.CompressFormat.JPEG, 100, stream);
            byte[] byteArray = stream.toByteArray();
            File file = new File(fileName);
            FileOutputStream fos = new FileOutputStream(file);
            try {
                fos.write(byteArray);
                fos.close();
                return file;
            } catch (Exception error) {
                return null;
            } finally {
                fos.close();
            }
        }
        return null;
    }

    private float getSizeFileMB(String path) {
        File file = new File(path);

        if (file.length() == 0) {
            InputStream inputStream = null;
            try {
                inputStream = context.getContentResolver().openInputStream(Uri.parse(path));
                assert inputStream != null;
                return ((float) inputStream.available() / (Constants.FACTOR_CONVERSION_MB));
            } catch (FileNotFoundException fileNotFoundException) {
                return 0;
            } catch (IOException ioException) {
                return 0;
            } finally {
                if (inputStream != null) {
                    try {
                        inputStream.close();
                    } catch (IOException e) {
                        Log.e(Constants.EXCEPTION_STRING, e.toString());
                    }
                }
            }

        } else {
            return ((float) file.length() / (Constants.FACTOR_CONVERSION_MB));
        }
    }

    @Override
    public String getSizeFile(String path) {
        File file = new File(path);
        float size = 0;
        DecimalFormat decimalFormat = new DecimalFormat(Constants.FORMAT_MB);

        if (file.length() == 0) {
            InputStream inputStream = null;
            try {
                inputStream = context.getContentResolver().openInputStream(Uri.parse(path));
                assert inputStream != null;
                size = (float) inputStream.available();
            } catch (FileNotFoundException fileNotFoundException) {
                Log.e(Constants.EXCEPTION_STRING, fileNotFoundException.toString());
            } catch (IOException ioException) {
                Log.e(Constants.EXCEPTION_STRING, ioException.toString());
            } finally {
                if (inputStream != null) {
                    try {
                        inputStream.close();
                    } catch (IOException e) {
                        Log.e(Constants.EXCEPTION_STRING, e.toString());
                    }
                }
            }
        } else {
            size = (float) file.length();
        }

        if (size >= Constants.FACTOR_CONVERSION_MB) {
            return decimalFormat.format(size / Constants.FACTOR_CONVERSION_MB) + Constants.MEGABYTE;
        } else {
            return decimalFormat.format(size / Constants.FACTOR_CONVERSION_KB) + Constants.KILOBYTE;
        }
    }

    @Override
    public String getExtensionFile(String path) {
        ContentResolver contentResolver = context.getContentResolver();
        MimeTypeMap mime = MimeTypeMap.getSingleton();
        String type = mime.getExtensionFromMimeType(contentResolver.getType(Uri.parse(path)));

        if (type == null) {
            type = path.substring(path.lastIndexOf(".") + 1, path.length());
        }

        return type;
    }

    @Override
    public String getContentTypeFile(String path) {

        String typeFile = getExtensionFile(path);
        return typeFile.equalsIgnoreCase("mp3") ? "audio/mpeg" : "image/" + typeFile;

    }

    @Override
    public String fileToBytesBase64(String attach) throws IOException {
        String fileContent = Constants.EMPTY_STRING;
        byte[] file;
        try {
            InputStream inputStream = new FileInputStream(attach);
            file = getByteArrayFromInputStream(inputStream);
            return Base64.encodeToString(file, Base64.DEFAULT);
        } catch (FileNotFoundException e) {
            InputStream inputStream = context.getContentResolver().openInputStream(Uri.parse(attach));
            file = getByteArrayFromInputStream(inputStream);
            return Base64.encodeToString(file, Base64.DEFAULT);
        } catch (IOException e) {
            Log.e(Constants.EXCEPTION_STRING, e.toString());
        }
        return fileContent;
    }

    @Override
    public byte[] getByteArrayFromInputStream(InputStream inputStream) throws IOException {
        ByteArrayOutputStream byteBuffer = new ByteArrayOutputStream();
        int bufferSize = inputStream.available();
        byte[] buffer = new byte[bufferSize];

        int len = 0;
        while ((len = inputStream.read(buffer)) != -1) {
            byteBuffer.write(buffer, 0, len);
        }
        return byteBuffer.toByteArray();
    }


    @Override
    public String getRealPath(String path) {
        String realPath = Constants.EMPTY_STRING;
        File file = new File(path);
        if (file.length() != 0) {
            realPath = file.getPath();
        }
        return realPath;
    }


    @Override
    public boolean verifyMaxMegaBytesArrayFiles(int valueMax, ArrayList<String> arrayFiles) {
        float fullSize = 0;
        for (String path : arrayFiles) {
            fullSize += getSizeFileMB(path);
        }
        return (fullSize <= valueMax);
    }

    @Override
    public ImageView getImageViewInstance() {
        SquareImageView imageViewCustom = new SquareImageView(context);
        LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WRAP_CONTENT, LinearLayout.LayoutParams.WRAP_CONTENT);
        lp.setMargins(15, 0, 0, 0);
        imageViewCustom.setLayoutParams(lp);
        imageViewCustom.setScaleType(ImageView.ScaleType.CENTER_CROP);
        return imageViewCustom;
    }

    @Override
    public byte[] fileToBytes(File file) {
        byte[] bytes = new byte[0];
        try (FileInputStream inputStream = new FileInputStream(file)) {
            bytes = new byte[inputStream.available()];

            //esta linea no se puede borrar asi lo recomienda sonar
            int currentBytesRead = 0;
            int totalBytesRead = 0;
            while ((currentBytesRead = inputStream.read(bytes)) > 0) {
                totalBytesRead += currentBytesRead;
            }
        } catch (IOException e) {
            Log.e(Constants.EXCEPTION_FILE_TO_BYTE, e.toString());
        }
        return bytes;
    }

    public String fileNotFound(String path) {
        byte[] fileOfBytes = null;
        try {
            InputStream inputStream = context.getContentResolver().openInputStream(Uri.parse(path));
            fileOfBytes = getByteArrayFromInputStream(inputStream);
            return evaluateFile(fileOfBytes);

        } catch (Exception e) {
            Log.e("Error", e.getMessage());
            return Constants.EMPTY_STRING;
        }
    }

    public String evaluateFile(byte[] fileOfBytes) {
        String realPath = Constants.EMPTY_STRING;
        if (fileOfBytes != null) {
            realPath = writeByte(fileOfBytes);
            return (realPath != null && !realPath.equalsIgnoreCase(Constants.EMPTY_STRING)) ? realPath : Constants.EMPTY_STRING;
        } else {
            return realPath;
        }
    }

    private String writeByte(byte[] bytes) {
        boolean successFile;
        OutputStream os = null;
        File tarjeta = Environment.getExternalStorageDirectory();
        File file = new File(tarjeta.getAbsolutePath(), Constants.TEMP_IMAGE_DAMAGE_FRAUDE + "_" + System.currentTimeMillis() + "_" + Constants.SUFFIX_FILE_IMAGE);
        try {
            os = new FileOutputStream(file);
            os.write(bytes);
            successFile = true;
        } catch (IOException e) {
            Log.e(TAG,   e.getMessage());
            successFile = false;

        } finally {
            try {
                if (os != null) {
                    os.close();
                }
            } catch (IOException e) {
                Log.e(TAG,   e.getMessage());
            }
        }
        return successFile ? file.getAbsolutePath() : Constants.EMPTY_STRING;
    }
}