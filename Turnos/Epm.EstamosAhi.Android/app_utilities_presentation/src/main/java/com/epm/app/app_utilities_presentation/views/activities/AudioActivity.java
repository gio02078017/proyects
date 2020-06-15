package com.epm.app.app_utilities_presentation.views.activities;

import android.Manifest;
import android.content.Intent;
import android.graphics.drawable.AnimationDrawable;
import android.graphics.drawable.Drawable;
import android.media.MediaPlayer;
import android.media.MediaRecorder;
import android.os.Bundle;
import android.os.CountDownTimer;
import android.os.Handler;
import androidx.annotation.Nullable;
import android.util.Log;
import android.view.View;
import android.widget.ImageButton;
import android.widget.ImageView;
import android.widget.ProgressBar;
import android.widget.TextView;
import android.widget.ToggleButton;

import com.epm.app.app_utilities_presentation.R;

import java.io.IOException;

import app.epm.com.utilities.helpers.FileManager;
import app.epm.com.utilities.helpers.IFileManager;
import app.epm.com.utilities.helpers.Permissions;
import app.epm.com.utilities.utils.Constants;
import app.epm.com.utilities.view.activities.BaseActivity;

public class AudioActivity extends BaseActivity {
    private MediaPlayer mPlayer;
    private AnimationDrawable animationDrawable;
    private ToggleButton tbtnRecordAndStop;
    private ImageButton play, deleteAudio;
    private ProgressBar progressBar;
    private TextView cancel, attachAudio, tvSeconsPlus, tvSeconsLess;
    private CountDownTimer mCountDownTimer;
    private MediaRecorder mRecorder = null;

    private String fileName = Constants.EMPTY_STRING;
    private boolean isReproduction = true, isRunning = false, isActive = true;
    private double countProgressPlusInit = 0, countProgressLessInit = 30;

    @Override
    protected void onCreate(@Nullable Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        loadDrawerLayout(R.id.generalDrawerLayout);
        loadSwipeDrawerLayout();
        setContentView(R.layout.activity_audio);
        loadViews();
        loadListenerToTheControl();
        IFileManager fileManager = new FileManager(this);
        this.fileName = fileManager.createPathForAudio();
        positionImage();
        setPermissions();
    }

    private void setPermissions() {
        String[] permissions = {Manifest.permission.RECORD_AUDIO, Manifest.permission.WRITE_EXTERNAL_STORAGE};
        Permissions.verifyPermissions(this, permissions);
    }

    @Override
    protected void onStop() {
        super.onStop();
        this.isActive = false;
    }

    @Override
    public void onPause() {
        super.onPause();
        if (mRecorder != null) {
            this.isRunning = false;
            this.mCountDownTimer.cancel();
            this.mRecorder.release();
            this.mRecorder = null;
            this.play.setBackgroundResource(R.mipmap.icon_play_green);
            this.play.setEnabled(true);
            this.deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
            this.deleteAudio.setEnabled(true);
            this.tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_gray);
            this.tbtnRecordAndStop.setEnabled(false);
        }

        this.deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);

        if (this.mPlayer != null) {
            this.mCountDownTimer.cancel();
            pausedReproducction();
            this.play.setBackgroundResource(R.mipmap.icon_play_green);
            this.play.setEnabled(true);
            this.deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
            this.deleteAudio.setEnabled(true);
            this.tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_gray);
            this.tbtnRecordAndStop.setEnabled(false);
        }
    }

    private void loadViews() {
        ImageView audioAnimation = findViewById(R.id.img_audio_animation);
        audioAnimation.setBackgroundResource(R.drawable.animation_audio);
        this.animationDrawable = (AnimationDrawable) audioAnimation.getBackground();
        this.tbtnRecordAndStop =  findViewById(R.id.tbtn_record_stop);
        this.play =  findViewById(R.id.btn_play);
        this.deleteAudio =  findViewById(R.id.btn_delete);
        this.progressBar =  findViewById(R.id.progress_bar_recorder_audio);
        this.cancel = findViewById(R.id.tv_cancel);
        this.attachAudio =  findViewById(R.id.tv_attach_audio);
        this.tvSeconsPlus =  findViewById(R.id.tv_plus_secons);
        this.tvSeconsLess =  findViewById(R.id.tv_less_secons);
    }

    private void loadListenerToTheControl() {
        this.tbtnRecordAndStop.setOnCheckedChangeListener((buttonView, isRecord) -> {
            if ((Permissions.isGrantedPermissions(AudioActivity.this, Manifest.permission.RECORD_AUDIO) &&
                    Permissions.isGrantedPermissions(AudioActivity.this, Manifest.permission.WRITE_EXTERNAL_STORAGE))) {
                managerRecord(isRecord);
            } else {
                setPermissions();
            }
        });

        this.play.setOnClickListener(v -> onPlay(isReproduction));

        this.deleteAudio.setOnClickListener(v -> onDelete());

        this.cancel.setOnClickListener(v -> onBackPressed());

        this.attachAudio.setOnClickListener(v -> {
            Intent intentAttachAudio = new Intent();
            intentAttachAudio.putExtra(Constants.PATH_ATTACH_AUDIO, fileName);
            setResult(Constants.AUDIO_CAPTURE, intentAttachAudio);
            finish();
        });
    }

    private void managerRecord(boolean isRecord) {
        if (isRecord) {
            tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_green);
            play.setBackgroundResource(R.mipmap.icon_play_gray);
            play.setEnabled(false);
            deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_gray);
            deleteAudio.setEnabled(false);
            attachAudio.setVisibility(View.GONE);
            clearComponents();
            onRecord(true);
        } else {
            attachAudio.setVisibility(View.VISIBLE);
            cancel.setVisibility(View.VISIBLE);
            play.setEnabled(true);
            deleteAudio.setEnabled(true);
            isRunning = false;
            onRecord(false);
        }
    }

    private void onRecord(boolean start) {
        this.play.setVisibility(View.VISIBLE);
        this.deleteAudio.setVisibility(View.VISIBLE);
        Log.e(AudioActivity.class.getName(), "aqui estoy " + countProgressPlusInit);
        if (start) {
            Log.e(AudioActivity.class.getName(), "entro al start");
            startProgress();
            startRecording();
            this.tbtnRecordAndStop.setEnabled(false);
            Handler handler = new Handler();
            handler.postDelayed(() -> tbtnRecordAndStop.setEnabled(true), 500);

        } else {
            Log.e(AudioActivity.class.getName(), "entro al stop");

            stopRecording();
        }
    }

    private void stopRecording() {
        Log.e(AudioActivity.class.getName(), "entro al stoprecording");
        //cancelo el conteo
        this.isRunning = false;
        this.play.setBackgroundResource(R.mipmap.icon_play_green);
        this.deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
        this.tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_gray);
        this.tbtnRecordAndStop.setEnabled(false);

        this.mCountDownTimer.cancel();
        Log.e(AudioActivity.class.getName(), "entro al intermedio stoprecording");
        if (this.mRecorder != null) {
            Log.e(AudioActivity.class.getName(), "entro al stoprecording antes del stop del if");
            Log.e(AudioActivity.class.getName(), "dio " + (mRecorder != null));
            this.mRecorder.stop();
            Log.e(AudioActivity.class.getName(), "entro al stoprecording despues del stop");
            this.mRecorder.release();
            this.mRecorder = null;
            countProgressLessInit = 0;

            this.tvSeconsLess.setText(String.valueOf(Math.round(this.countProgressPlusInit)) + "s");
            this.tvSeconsPlus.setText("0s");
        }
    }

    private void startProgress() {
        Log.e(AudioActivity.class.getName(), "entro al sstartprogress");
        this.mCountDownTimer = new CountDownTimer(Constants.MAX_DURATION, 100) {
            @Override
            public void onTick(long millisUntilFinished) {
                progressBar.setProgress(progressBar.getProgress() + 100);
                double count = progressBar.getProgress() / 1000.0;
                countProgressPlusInit = +count;
                countProgressLessInit = 30 - count;
                changeTextView();
            }

            @Override
            public void onFinish() {
                isRunning = false;
                if (progressBar.getProgress() > 0) {
                    //termino de llenar el progress
                    progressBar.setProgress(progressBar.getProgress() + 1000);
                    play.setBackgroundResource(R.mipmap.icon_play_green);
                    deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
                    tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_gray);
                    tbtnRecordAndStop.setChecked(false);
                }
            }
        };

        mCountDownTimer.start();
    }

    private void startRecording() {
        Log.e(AudioActivity.class.getName(), "entro al startrecording");

        this.isRunning = true;
        this.mRecorder = new MediaRecorder();
        this.mRecorder.setMaxDuration(Constants.MAX_DURATION);
        //Define la fuente de audio.
        this.mRecorder.setAudioSource(MediaRecorder.AudioSource.MIC);
        //Define el formato de salida.
        this.mRecorder.setOutputFormat(MediaRecorder.OutputFormat.MPEG_4);
        //Define la codificación de vídeo.
        this.mRecorder.setAudioEncoder(MediaRecorder.OutputFormat.AMR_NB);
        //Establece la ruta del archivo de salida para ser producido.
        this.mRecorder.setOutputFile(fileName);

        try {
            this.mRecorder.prepare();
            this.mRecorder.start();
            this.cancel.setVisibility(View.GONE);

        } catch (IOException e) {
            Log.e(AudioActivity.class.getName(), "prepare() failed");
        }
    }

    private void onDelete() {
        if (this.mPlayer != null) {
            this.mPlayer.stop();
            this.mPlayer.release();
        }
        this.isRunning = false;
        this.mCountDownTimer.cancel();
        clearComponents();
        this.attachAudio.setVisibility(View.GONE);
        this.play.setVisibility(View.GONE);
        this.deleteAudio.setVisibility(View.GONE);
        this.tbtnRecordAndStop.setChecked(false);
        this.tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_microphone_audio_green);
        this.tbtnRecordAndStop.setEnabled(true);
        this.animationDrawable.selectDrawable(0);

        this.countProgressPlusInit = 0;
        this.countProgressLessInit = 30;

        changeTextView();
    }

    private void clearComponents() {
        if (this.countProgressLessInit != 30) {
            this.countProgressLessInit = 30;
            String countSecons = String.valueOf(countProgressPlusInit) + "s";
            this.tvSeconsPlus.setText(countSecons);
        }

        this.tvSeconsPlus.setText("");
        this.progressBar.setMax(30000);
        this.progressBar.setProgress(0);
        this.mPlayer = null;
        this.mRecorder = null;
    }

    private void positionImage() {
        Thread thread = new Thread(() -> {
            while (isActive) {
                if (isRunning) {
                    runOnUiThread(() -> {
                        int position = getNextPosition();
                        if (position >= animationDrawable.getNumberOfFrames()) {
                            animationDrawable.selectDrawable(0);
                        } else {
                            animationDrawable.selectDrawable(position);
                        }
                    });
                }
                try {
                    Thread.sleep(200);
                } catch (InterruptedException e) {
                    Log.i("Arcgis", "Interrupted");
                    Thread.currentThread().interrupt();
                }
            }
        });

        thread.start();
    }

    private int getNextPosition() {
        Drawable drawable = animationDrawable.getCurrent();
        for (int i = 0; i < animationDrawable.getNumberOfFrames(); i++) {
            if (animationDrawable.getFrame(i) == drawable) {
                return i + 1;
            }
        }

        return 0;
    }

    private void onPlay(boolean isReproduction) {
        play.setBackgroundResource(isReproduction ? R.mipmap.icon_pause_green : R.mipmap.icon_play_green);
        deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
        if (isReproduction) {
            if (mPlayer == null) {
                startPlaying();
            } else {
                continueReproduction();
            }

        } else {
            pausedReproducction();
        }
        this.isReproduction = !isReproduction;
    }

    private void pausedReproducction() {
        if (this.mPlayer != null) {
            this.isRunning = false;
            this.mPlayer.pause();
            this.mCountDownTimer.cancel();
        }
    }

    private void continueReproduction() {
        this.isRunning = true;
        this.mPlayer.seekTo((int) (countProgressPlusInit * 1000) - 100);
        this.mPlayer.start();
        startProgressPlay();
    }

    private void startPlaying() {
        this.isRunning = true;
        this.progressBar.setProgress(0);
        this.progressBar.setMax((int) (this.countProgressPlusInit * 1000) - 100);
        this.mPlayer = new MediaPlayer();
        try {
            this.mPlayer.setDataSource(this.fileName);
            this.mPlayer.prepare();
            this.mPlayer.start();
            this.progressBar.setProgress(0);
            this.countProgressLessInit = this.countProgressPlusInit;
            this.countProgressPlusInit = 0;
            startProgressPlay();
        } catch (IOException e) {
            Log.e(AudioActivity.class.getName(), "prepare() failed");
        }

        //listener para detectar el final del audio
        this.mPlayer.setOnCompletionListener(
                new MediaPlayer.OnCompletionListener() {
                    @Override
                    public void onCompletion(MediaPlayer mp) {
                        isReproduction = true;
                        play.setBackgroundResource(R.mipmap.icon_play_green);
                        mPlayer = null;
                    }
                });
    }

    private void startProgressPlay() {
        this.mCountDownTimer = new CountDownTimer((long) ((this.countProgressLessInit - this.countProgressPlusInit) * 1000), 100) {
            @Override
            public void onTick(long millisUntilFinished) {
                progressBar.setProgress(progressBar.getProgress() + 100);
                countProgressPlusInit += 0.1;
            }

            @Override
            public void onFinish() {
                isRunning = false;
                if (progressBar.getProgress() > 0) {
                    //termino de llenar el progress
                    countProgressPlusInit += 0.1;
                    play.setBackgroundResource(R.mipmap.icon_play_green);
                    deleteAudio.setBackgroundResource(R.mipmap.icon_delete_audio_green);
                    tbtnRecordAndStop.setBackgroundResource(R.mipmap.icon_stop_gray);
                    tbtnRecordAndStop.setChecked(false);
                }
            }
        };

        mCountDownTimer.start();
    }

    private void changeTextView() {
        String countSeconsPlus = String.valueOf(Math.round(this.countProgressPlusInit) + "s");
        String countSeconsLess = String.valueOf(Math.round(this.countProgressLessInit) + "s");

        this.tvSeconsPlus.setText(countSeconsPlus);
        this.tvSeconsLess.setText(countSeconsLess);
    }
}