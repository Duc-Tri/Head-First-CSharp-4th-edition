using Stopwatch.Model;

namespace Stopwatch.ViewModel
{

    public class StopwatchViewModel
    {
        private StopwatchModel _model;
        public StopwatchModel Model {
            get
            {
                return _model;
            }
        }

        public void Start()        
        {
            if(!_model.Running)
                _model.Running=true;
        }

        public void StartStop()=> throw new NotImplementedException();
        public void Reset()
        {
            _model.Reset();
        }

        public string Hours=> _model.Elapsed.Hours.ToString("D2");
        public string Minutes=> _model.Elapsed.Minutes.ToString("D2");
        public string Seconds=> _model.Elapsed.Seconds.ToString("D2");
        public object Tenths=> (_model.Elapsed.Milliseconds / 100M).ToString("D2");

    }
    
}