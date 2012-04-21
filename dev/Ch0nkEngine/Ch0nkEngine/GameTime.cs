using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Ch0nkEngine
{
    /// <summary>
    /// A mechanism for tracking elapsed time.
    /// </summary>
    public class GameTime
    {
        #region Public Interface

        /// <summary>
        /// Initializes a new instance of the <see cref="Clock"/> class.
        /// </summary>
        public GameTime()
        {
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _stopwatch.Start();
            _isRunning = true;
            _timeInLast = 0;
            _currentDelta = 0;
        }

        public long ElapsedMiliseconds
        {
            get { return _currentDelta; }
        }

        /// <summary>
        /// Updates the clock.
        /// </summary>
        public void Update()
        {
            if (_isRunning)
            {
                long count = Stopwatch.GetTimestamp();
                _currentDelta = count - _timeInLast;
                _timeInLast = count;

            }

        }

        #endregion

        #region Implementation Detail

        private bool _isRunning;
        private readonly long _frequency;
        private Stopwatch _stopwatch;
        private long _timeInLast;
        private long _currentDelta;

        #endregion
    }
}
