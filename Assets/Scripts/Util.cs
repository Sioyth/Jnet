using System;
using System.Threading;
using System.Threading.Tasks;

public class JTimer
{
    private float _interval;
    private DateTime _currentTime;
    private DateTime _lastTime;
    private TimeSpan _timePassed;
    private bool _isRunning;
    private CancellationToken _cToken;
    private CancellationTokenSource _cTokenSource;
    public Action OnElapsedTime { get => _onElapsedTime; set => _onElapsedTime = value; }

    private event Action _onElapsedTime;

    public JTimer(float time)
    { 
        _interval = time;
        _currentTime = DateTime.Now;
        _lastTime = DateTime.Now;
        _timePassed = new TimeSpan();
        _cTokenSource = new CancellationTokenSource();
        _cToken = _cTokenSource.Token;
    }

    public async void Start()
    {
       // Stop();
        Reset();

        DateTime now = DateTime.Now;

        if (_isRunning)
            return;
        
        await Task.Run(() =>
        {
            _isRunning = true;
            while (_isRunning)
            {
                _currentTime = DateTime.Now;
                _timePassed += _currentTime - _lastTime;
                _lastTime = _currentTime;

                UnityEngine.Debug.Log(_timePassed.Seconds);

                if (_timePassed.Seconds >= _interval)
                {
                    _onElapsedTime?.Invoke();
                    _isRunning = false;
                }
            }
        }, _cToken);
    }

    public void Reset()
    {
        _currentTime = DateTime.Now;
        _lastTime = DateTime.Now;
        _timePassed = TimeSpan.Zero;
    }

    public void Stop()
    {
        _cTokenSource.Cancel();
        _isRunning = false;
    }

    public void Resume()
    {
        _isRunning = true;
    }

    




}