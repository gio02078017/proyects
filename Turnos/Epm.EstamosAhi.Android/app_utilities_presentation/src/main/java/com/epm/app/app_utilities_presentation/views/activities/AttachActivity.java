package com.epm.app.app_utilities_presentation.views.activities;

import android.Manifest;
import android.content.ClipData;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.pm.PackageManager;
import android.content.pm.ResolveInfo;
import android.net.Uri;
import android.os.Build;
import android.os.Bundle;
import android.provider.MediaStore;
import androidx.annotation.NonNull;
import androidx.core.content.FileProvider;
import androidx.recyclerview.widget.LinearLayoutManager;
import androidx.recyclerview.widget.RecyclerView;
import androidx.appcompat.widget.Toolbar;
import android.view.View;
import android.view.ViewTreeObserver;
import android.widget.Button;
import android.widget.CheckBox;
import android.widget.CompoundButton;
import android.widget.ImageView;
import android.widget.LinearLayout;
import android.widget.RelativeLayout;
import android.widget.TextView;

import com.epm.app.app_utilities_presentation.R;
import com.epm.app.app_utilities_presentation.views.adapter.RecyclerViewAttachImageAdapter;

import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.presenters.BasePresenter;
import app.epm.com.utilities.utils.Constants;

import com.epm.app.app_utilities_presentation.utils.TipoAdjunto;

import app.epm.com.utilities.view.activities.BaseActivity;


public abstract class AttachActivity<T extends BasePresenter> extends BaseActivity<T> {
    private ImageView attach_ImageViewMicrophone, attach_ImageViewGallery, attach_ImageViewCamera, attach_ImageViewResumen, attach_ImageViewPoints, attach_ivLine;
    private Button attach_ButtonAction;
    private RelativeLayout attach_RelativeLayoutAnonymous;
    private TextView attach_TextViewTitleService, attach_TextViewMessage, attach_TextViewCount;
    private Toolbar attach_ToolbarApp;
    private CheckBox attach_CheckBox_anonymous;
    private LinearLayout attach_LinearLayoutDescription;
    private LinearLayout attach_ContenedorAdjuntos;
    private IFileManager attach_FileManager;
    private RecyclerView attach_RecyclerViewItems;
    private RecyclerViewAttachImageAdapter recyclerViewAttachImageAdapter;

    private int maxAdjuntos = 0, maxMegas = 0;
    private String modulo = Constants.EMPTY_STRING;
    private TipoAdjunto tipoAdjunto;
    private ArrayList<String> arrayFiles;
    private File photoFile;
    private boolean isCheck;
    private boolean openCalls;
    private boolean controlAttachAudio, controlAttachImageGallery, controlAttachImageCamera,controlAttachResumen;

    public Boolean getAnonymous() {
        return this.attach_CheckBox_anonymous.isChecked();
    }

    public void setAnonymous(Boolean anonymous) {
        this.attach_CheckBox_anonymous.setChecked(anonymous);
        this.attach_CheckBox_anonymous.setButtonDrawable(anonymous ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
        loadOnCheckBoxChangedToTheControlAttachCheckBoxbAnonymous(anonymous);
    }

    private void loadOnCheckBoxChangedToTheControlAttachCheckBoxbAnonymous(Boolean anonymous) {
        isCheck = anonymous;
        attach_CheckBox_anonymous.setOnCheckedChangeListener(new CompoundButton.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(CompoundButton compoundButton, boolean isChecked) {
                attach_CheckBox_anonymous.setButtonDrawable(isChecked ? R.mipmap.checkbox_on : R.mipmap.checkbox_off);
                isCheck = isChecked;
            }
        });
    }

    public void setArrayFiles(ArrayList<String> arrayFiles) {
        this.arrayFiles = arrayFiles;
    }

    public AttachActivity(String modulo, int maxAdjuntos, int maxMegas) {
        this.modulo = modulo;
        this.maxAdjuntos = maxAdjuntos;
        this.maxMegas = maxMegas;
        this.tipoAdjunto = TipoAdjunto.ninguno;
        this.arrayFiles = new ArrayList<>(maxAdjuntos);
    }

    public abstract void showResume(ArrayList<String> filesNames);

    public abstract void clickButton(ArrayList<String> filesNames);

    public abstract void onBackPressed(ArrayList<String> filesNames);

    public abstract String getToolBarNameModule();

    public abstract String getTextViewTitleAttach();

    public abstract String getTextViewDescribeAttach();

    public abstract String getButtonTextAttach();

    public abstract int getImageViewLine();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_attach);
        loadDrawerLayout(R.id.generalDrawerLayout);
        this.attach_FileManager = new FileManager(this);
        createProgressDialog();
        loadViews();
        loadViewsCustom();
        SetTextViewsCustom();
        callAdapter();
        observerLayoutGetSize();
    }

    @Override
    public void onBackPressed() {
        onBackPressed(arrayFiles);
    }

    @Override
    public Intent getDefaultIntent(Intent intent) {
        if (openCalls) {
            return intent;
        } else {
            return super.getDefaultIntent(intent);
        }
    }

    @Override
    protected void onResume() {
        super.onResume();
        this.controlAttachAudio = false;
        this.controlAttachImageGallery = false;
        this.controlAttachImageCamera = false;
        this.controlAttachResumen = false;
    }

    @Override
    protected void onActivityResult(int requestCode, int resultCode, Intent data) {
        if (resultCode == 0) return;

        // Para la galeria de versiones mayores o iguales a Kitkat
        if (requestCode == Constants.GALLERY_KITKAT_AND_HIGHER) {
            resultGalleryKitkatAndHigher(data, data.getData());
        }

        // Para la galeria de versiones menores a Kitkat.
        else if (requestCode == Constants.GALLERY_JELLY_BEAN_AND_LESS) {
            resultGalleryJellyBeanAndLess(data.getData());
        }

        // Para la camara.
        else if (requestCode == Constants.CAMERA_CAPTURE) {
            resultCameraCapture();
        }

        // para el audio.
        else if (resultCode == Constants.AUDIO_CAPTURE) {
            resultAudioCapture(data);
        } else {
            super.onActivityResult(requestCode, resultCode, data);
        }
    }

    private void resultAudioCapture(Intent data) {
        String pathAttachAudio = data.getStringExtra(Constants.PATH_ATTACH_AUDIO);
        setArrayConFilesNames(pathAttachAudio);
    }

    private void resultCameraCapture() {
        if (photoFile != null) {
            setArrayConFilesNames(photoFile.getPath());
        }
    }

    private void resultGalleryKitkatAndHigher(Intent data, Uri uri) {
        // Cuando se escogen más de una imagen.
        ClipData clipData = data.getClipData();
        if (uri == null) {
            for (int i = 0; i < clipData.getItemCount(); i++) {
                if (isValidLengthFiles()) {
                    grantUriPermission(getPackageName(), clipData.getItemAt(i).getUri(), Intent.FLAG_GRANT_READ_URI_PERMISSION);
                    setArrayConFilesNames(clipData.getItemAt(i).getUri().toString());
                } else {
                    return;
                }
            }
        } else {
            //Cuando se escoge una imagen.
            grantUriPermission(getPackageName(), uri, Intent.FLAG_GRANT_READ_URI_PERMISSION);
            setArrayConFilesNames(uri.toString());
        }
    }

    private void resultGalleryJellyBeanAndLess(Uri uri) {
        grantUriPermission(getPackageName(), uri, Intent.FLAG_GRANT_READ_URI_PERMISSION);
        setArrayConFilesNames(uri.toString());
    }

    private void setArrayConFilesNames(String path) {
        arrayFiles.add(path);
        setCount();
        this.recyclerViewAttachImageAdapter.setFiles(arrayFiles);
        this.recyclerViewAttachImageAdapter.notifyDataSetChanged();
    }

    @Override
    public void onRequestPermissionsResult(int requestCode, @NonNull String[] permissions, @NonNull int[] grantResults) {

        if (requestCode == Constants.REQUEST_CODE_PERMISSION && this.tipoAdjunto == TipoAdjunto.camera) {
            if (Permissions.isGrantedPermissions(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
                dispatchTakePictureIntent();
            }
        }

        if (requestCode == Constants.REQUEST_CODE_PERMISSION && this.tipoAdjunto == TipoAdjunto.gallery) {
            if (Permissions.isGrantedPermissions(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
                showGalleryIntent();
            }
        }

        this.tipoAdjunto = TipoAdjunto.ninguno;
        super.onRequestPermissionsResult(requestCode, permissions, grantResults);
    }

    private void callAdapter() {
        this.recyclerViewAttachImageAdapter = new RecyclerViewAttachImageAdapter(AttachActivity.this) {
            @Override
            public void setCount() {
                AttachActivity.this.setCount();
            }
        };

        attach_RecyclerViewItems.setAdapter(recyclerViewAttachImageAdapter);
    }

    private void loadViews() {
        this.attach_ToolbarApp = findViewById(R.id.toolbar);
        this.attach_RelativeLayoutAnonymous =  findViewById(R.id.attach_rlAnonymous);
        this.attach_CheckBox_anonymous =  findViewById(R.id.img_state_anonymous);
        this.attach_TextViewMessage =  findViewById(R.id.attach_tvMessage);
        this.attach_TextViewTitleService =  findViewById(R.id.attach_tvTitleService);
        this.attach_ImageViewMicrophone =  findViewById(R.id.attach_ivMicrophone);
        this.attach_ImageViewGallery =  findViewById(R.id.attach_ivGallery);
        this.attach_ImageViewPoints =  findViewById(R.id.attach_ivPoints);
        this.attach_ImageViewCamera =  findViewById(R.id.attach_ivCamera);
        this.attach_ImageViewResumen =  findViewById(R.id.attach_ivResumen);
        this.attach_ivLine =  findViewById(R.id.attach_ivLine);
        this.attach_ButtonAction =  findViewById(R.id.attach_btn);
        this.attach_RecyclerViewItems =  findViewById(R.id.attach_rvItems);
        this.attach_RecyclerViewItems.setHasFixedSize(true);
        RecyclerView.LayoutManager layoutManager = new LinearLayoutManager(this, LinearLayoutManager.HORIZONTAL, false);
        this.attach_RecyclerViewItems.setLayoutManager(layoutManager);
        this.attach_TextViewCount =  findViewById(R.id.tv_count);
        this.attach_LinearLayoutDescription =  findViewById(R.id.attach_llDescription);
        this.attach_ContenedorAdjuntos = findViewById(R.id.attach_ContenedorAdjuntos);
        setListener();
        loadToolbar();
    }

    private void observerLayoutGetSize() {
        ViewTreeObserver observer = this.attach_ContenedorAdjuntos.getViewTreeObserver();
        observer.addOnGlobalLayoutListener(new ViewTreeObserver.OnGlobalLayoutListener() {
            @Override
            public void onGlobalLayout() {
                attach_ContenedorAdjuntos.getViewTreeObserver().removeGlobalOnLayoutListener(this);
                recyclerViewAttachImageAdapter.setSize(attach_ContenedorAdjuntos.getWidth(),
                        attach_ContenedorAdjuntos.getHeight());
                setCount();
                recyclerViewAttachImageAdapter.setFiles(arrayFiles);
                recyclerViewAttachImageAdapter.notifyDataSetChanged();
            }
        });
    }

    private void loadViewsCustom() {
        switch (this.modulo) {
            case Constants.REPORT_DANIOS:
                loadViewReporteDanios();
                break;
            case Constants.REPORT_FRAUDES:
                loadViewReporteFraudes();
                break;
            case Constants.CONTACTO:
                loadViewsContacto();
                break;
            default:
                break;
        }
    }

    private void SetTextViewsCustom() {
        this.attach_TextViewTitleService.setText(getTextViewTitleAttach());
        this.attach_TextViewMessage.setText(getTextViewDescribeAttach());
        this.attach_ButtonAction.setText(getButtonTextAttach());
        this.attach_ivLine.setImageResource(getImageViewLine());
    }

    private void loadViewsContacto() {
        this.attach_RelativeLayoutAnonymous.setVisibility(View.GONE);
        this.attach_ButtonAction.setEnabled(true);
        this.attach_ButtonAction.setBackgroundResource(R.color.button_green);
    }

    private void loadViewReporteFraudes() {
        this.attach_RelativeLayoutAnonymous.setVisibility(View.VISIBLE);
        this.attach_ButtonAction.setEnabled(true);
        this.attach_ButtonAction.setBackgroundResource(R.color.button_green);
    }

    private void loadToolbar() {
        setSupportActionBar(attach_ToolbarApp);
        attach_ToolbarApp.setNavigationIcon(R.mipmap.icon_right_arrow_green);
        getSupportActionBar().setDisplayShowTitleEnabled(false);
        this.attach_ToolbarApp.setNavigationOnClickListener(view -> onBackPressed(arrayFiles));
    }

    private void setListener() {
        this.attach_ImageViewMicrophone.setOnClickListener(v -> showAudio());

        this.attach_ImageViewGallery.setOnClickListener(v -> showGallery());

        this.attach_ImageViewCamera.setOnClickListener(v -> showCamera());

        this.attach_ImageViewResumen.setOnClickListener(v -> {
            tipoAdjunto = TipoAdjunto.ninguno;
            if(!controlAttachResumen){
                showResume(arrayFiles);
                controlAttachResumen = true;
            }
        });

        this.attach_ButtonAction.setOnClickListener(v -> clickButton(arrayFiles));
    }

    private void showAudio() {
        this.tipoAdjunto = TipoAdjunto.audio;
        if (isValidLengthFiles() && !controlAttachAudio) {
            showAudioIntent();
        }
    }

    private void showAudioIntent() {
        openCalls(true);
        Intent intent = new Intent(AttachActivity.this, AudioActivity.class);
        intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
        startActivityWithOutDoubleClick(intent, Constants.AUDIO_CAPTURE);
        openCalls(false);
        controlAttachAudio = true;
    }

    private void showCamera() {
        this.tipoAdjunto = TipoAdjunto.camera;
        if (Permissions.isGrantedPermissions(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
            if (isValidLengthFiles() && !controlAttachImageCamera) {
                dispatchTakePictureIntent();
            }
        } else {
            String[] permissions = {Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE};
            Permissions.verifyPermissions(this, permissions);
        }
    }

    private boolean isValidLengthFiles() {
        boolean isValid = verifyMaxSizeArrayFiles();
        if (isValid) {
            isValid = attach_FileManager.verifyMaxMegaBytesArrayFiles(this.maxMegas, arrayFiles);
            if (!isValid) {
                getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(R.string.not_attach_more_imagenes_size) + this.maxMegas + " MB.",
                        false, R.string.text_aceptar, getDefaulPositiveButtonOnClick(), null);
            }
        } else {
            getCustomAlertDialog().showAlertDialog(getName(), getResources().getString(R.string.not_attach_more_imagenes) + this.maxAdjuntos + " archivos.",
                    false, R.string.text_aceptar, getDefaulPositiveButtonOnClick(), null);

        }

        return isValid;
    }

    private boolean verifyMaxSizeArrayFiles() {
        return (this.arrayFiles.size() < this.maxAdjuntos);
    }

    private DialogInterface.OnClickListener getDefaulPositiveButtonOnClick() {
        return new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                dialog.dismiss();
            }
        };
    }

    private void loadViewReporteDanios() {
        this.attach_RelativeLayoutAnonymous.setVisibility(View.GONE);
        this.attach_ImageViewMicrophone.setVisibility(View.VISIBLE);
        this.attach_ImageViewGallery.setVisibility(View.VISIBLE);
        this.attach_ImageViewCamera.setVisibility(View.VISIBLE);
        this.attach_ImageViewResumen.setVisibility(View.VISIBLE);
        this.attach_ImageViewPoints.setVisibility(View.VISIBLE);
        this.attach_ButtonAction.setEnabled(true);
        this.attach_ButtonAction.setBackgroundResource(R.color.button_green);
    }

    private void dispatchTakePictureIntent() {
        Intent takePictureIntent = new Intent(MediaStore.ACTION_IMAGE_CAPTURE);
        photoFile = null;
        if (takePictureIntent.resolveActivity(getPackageManager()) != null) {
            try {
                photoFile = attach_FileManager.createImageFile();
            } catch (IOException ex) {
                openCalls(false);
                ex.getStackTrace();
            }

            if (photoFile != null) {
                Uri photoURI = FileProvider.getUriForFile(this,
                        "com.epm.app",
                        photoFile);

                List<ResolveInfo> resInfoList = getPackageManager().queryIntentActivities(takePictureIntent, PackageManager.MATCH_DEFAULT_ONLY);
                for (ResolveInfo resolveInfo : resInfoList) {
                    String packageName = resolveInfo.activityInfo.packageName;
                    grantUriPermission(packageName, photoURI, Intent.FLAG_GRANT_WRITE_URI_PERMISSION | Intent.FLAG_GRANT_READ_URI_PERMISSION);
                }
                takePictureIntent.putExtra(MediaStore.EXTRA_OUTPUT, photoURI);
                takePictureIntent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
                openCalls(true);
                super.startActivityWithOutDoubleClick(takePictureIntent, Constants.CAMERA_CAPTURE);
                openCalls(false);
            }
            openCalls(false);
            controlAttachImageCamera = true;
        }
    }

    private void showGalleryIntent() {
        Intent intent = new Intent(Intent.ACTION_PICK, android.provider.MediaStore.Images.Media.EXTERNAL_CONTENT_URI);
        intent.setType("image/*");
        intent.setAction(Intent.ACTION_GET_CONTENT);
        intent.addCategory(Intent.CATEGORY_OPENABLE);

        if (Build.VERSION.SDK_INT < 19) {
            openCalls(true);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivityWithOutDoubleClick(intent, Constants.GALLERY_JELLY_BEAN_AND_LESS);
            openCalls(false);
        } else {
            openCalls(true);
            String[] mimetypes = {"image/*"};
            intent.putExtra(Intent.EXTRA_MIME_TYPES, mimetypes);
            intent.putExtra(Intent.EXTRA_ALLOW_MULTIPLE, true);
            intent.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);
            startActivityWithOutDoubleClick(intent, Constants.GALLERY_KITKAT_AND_HIGHER);
            openCalls(false);
        }
        controlAttachImageGallery = true;
        openCalls(false);
    }

    private void showGallery() {
        this.tipoAdjunto = TipoAdjunto.gallery;
        if (Permissions.isGrantedPermissions(this, Manifest.permission.WRITE_EXTERNAL_STORAGE)) {
            if (isValidLengthFiles() && !controlAttachImageGallery) {
                showGalleryIntent();
            }
        } else {
            String[] permissions = {Manifest.permission.WRITE_EXTERNAL_STORAGE, Manifest.permission.READ_EXTERNAL_STORAGE};
            Permissions.verifyPermissions(this, permissions);
        }
    }

    private void setCount() {
        String lengthArray = arrayFiles.size() + "/" + maxAdjuntos;
        attach_TextViewCount.setText(lengthArray);
        if (arrayFiles.size() > 0) {
            attach_TextViewCount.setVisibility(View.VISIBLE);
            attach_LinearLayoutDescription.setVisibility(View.GONE);
            attach_RecyclerViewItems.setVisibility(View.VISIBLE);
        } else {
            attach_TextViewCount.setVisibility(View.GONE);
            attach_LinearLayoutDescription.setVisibility(View.VISIBLE);
            attach_RecyclerViewItems.setVisibility(View.GONE);
        }
    }

    private void openCalls(boolean openCalls) {
        this.openCalls = openCalls;
    }
}