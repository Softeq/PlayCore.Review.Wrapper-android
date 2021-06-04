package com.softeq.playcorewrapper;

import android.app.Activity;

import androidx.annotation.NonNull;

import com.google.android.play.core.common.PlayCoreDialogWrapperActivity;
import com.google.android.play.core.review.ReviewInfo;
import com.google.android.play.core.review.ReviewManager;
import com.google.android.play.core.review.ReviewManagerFactory;
import com.google.android.play.core.tasks.OnCompleteListener;
import com.google.android.play.core.tasks.Task;

public class RequestReviewService {
    public void requestReview(Activity activity, IReviewListener reviewListener) {
        PlayCoreDialogWrapperActivity
        ReviewManager rm = ReviewManagerFactory.create(activity);
        rm.requestReviewFlow().addOnCompleteListener(new OnCompleteListener<ReviewInfo>() {
            @Override
            public void onComplete(@NonNull Task<ReviewInfo> task) {
                if (task.isSuccessful()) {
                    ReviewInfo ri = task.getResult();
                    Task<Void> flow = rm.launchReviewFlow(activity, ri);
                    flow.addOnCompleteListener(launchTask -> {
                        if (launchTask.isSuccessful()) {
                            reviewListener.onSuccess();
                        } else {
                            reviewListener.onError();
                        }
                    });
                } else {
                    reviewListener.onError();
                }
            }
        });
    }
}