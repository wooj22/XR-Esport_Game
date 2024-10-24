using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class BalloonVideoManager : MonoBehaviour
{
    // VideoPlayer�� RawImage ����
    public VideoPlayer videoPlayer_front;
    public VideoPlayer videoPlayer_left;
    public VideoPlayer videoPlayer_right;

    public RawImage rawImage_front;
    public RawImage rawImage_left;
    public RawImage rawImage_right;

    void Start()
    {
        // �� ���� �÷��̾� �غ� �̺�Ʈ ����
        videoPlayer_front.prepareCompleted += OnVideoPrepared1;
        videoPlayer_left.prepareCompleted += OnVideoPrepared2;
        videoPlayer_right.prepareCompleted += OnVideoPrepared3;

        // ���� ���� �ε� �� �غ�
        videoPlayer_front.url = Application.streamingAssetsPath + "/balloon_front_.mp4";
        videoPlayer_left.url = Application.streamingAssetsPath + "/balloon_L_R.mp4";
        videoPlayer_right.url = Application.streamingAssetsPath + "/balloon_L_R.mp4";

        videoPlayer_front.Prepare();
        videoPlayer_left.Prepare();
        videoPlayer_right.Prepare();
    }


    // �� ���� �غ� �Ϸ� �� ȣ��Ǵ� �޼���
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
