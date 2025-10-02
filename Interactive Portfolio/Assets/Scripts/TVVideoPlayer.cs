using UnityEngine;
using UnityEngine.Video;

public class TVVideoPlayer : MonoBehaviour
{
    public VideoPlayer _videoPlayer;

    public VideoClip[] _clips;

    private int _clipIndex;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _clipIndex = 0;
    }

    private void Start()
    {
        _videoPlayer.clip = _clips[_clipIndex];
    }

    private void Update()
    {

        if ((float)_videoPlayer.frame == _videoPlayer.clip.frameCount - 2) 
        {
            _clipIndex++;
            if (_clipIndex >= _clips.Length) 
            {
                _clipIndex = 0;
            }

            _videoPlayer.clip = _clips[_clipIndex];
        }
    }

}
