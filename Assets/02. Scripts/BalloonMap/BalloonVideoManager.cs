using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BalloonVideoManager : MonoBehaviour
{
    // VideoPlayer와 RawImage 선언
    public VideoPlayer videoPlayer_front;
    public VideoPlayer videoPlayer_left;
    public VideoPlayer videoPlayer_right;

    public RawImage rawImage_front;
    public RawImage rawImage_left;
    public RawImage rawImage_right;

    void Start()
    {
        // 각 비디오 플레이어 준비 이벤트 설정
        videoPlayer_front.prepareCompleted += OnVideoPrepared1;
        videoPlayer_left.prepareCompleted += OnVideoPrepared2;
        videoPlayer_right.prepareCompleted += OnVideoPrepared3;

        // 비디오 파일 로드 및 준비
        videoPlayer_front.url = Application.streamingAssetsPath + "/balloon_front_.mp4";
        videoPlayer_left.url = Application.streamingAssetsPath + "/balloon_L_R.mp4";
        videoPlayer_right.url = Application.streamingAssetsPath + "/balloon_L_R.mp4";

        videoPlayer_front.Prepare();
        videoPlayer_left.Prepare();
        videoPlayer_right.Prepare();
    }


    // 각 비디오 준비 완료 시 호출되는 메서드
    void OnVideoPrepared1(VideoPlayer vp)
    {
        rawImage_front.texture = videoPlayer_front.targetTexture;
        videoPlayer_front.Play();
    }

    
    void OnVideoPrepared2(VideoPlayer vp)
    {
        rawImage_left.texture = videoPlayer_left.targetTexture;
        videoPlayer_left.Play();
    }
    
    void OnVideoPrepared3(VideoPlayer vp)
    {
        rawImage_right.texture = videoPlayer_right.targetTexture;
        videoPlayer_right.Play();
    }
}
