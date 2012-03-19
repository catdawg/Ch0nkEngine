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
        }

        public long ElapsedMiliseconds
        {
            get { return _stopwatch.ElapsedMilliseconds; }
        }

        /// <summary>
        /// Updates the clock.
        /// </summary>
        /// <returns>The time, in seconds, that elapsed since the previous update.</returns>
        public float Update()
        {
            float result = 0.0f;
            if (_isRunning)
            {
                long last = _count;
                _count = Stopwatch.GetTimestamp();
                
                result = (float)(_count - last) / _frequency;
            }

            return result;
        }

        #endregion

        #region Implementation Detail

        private bool _isRunning;
        private readonly long _frequency;
        private long _count;
        private Stopwatch _stopwatch;

        #endregion
    }
}
