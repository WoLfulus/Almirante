/// 
/// The MIT License (MIT)
/// 
/// Copyright (c) 2014 João Francisco Biondo Trinca <wolfulus@gmail.com>
/// 
/// Permission is hereby granted, free of charge, to any person obtaining a copy
/// of this software and associated documentation files (the "Software"), to deal
/// in the Software without restriction, including without limitation the rights
/// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
/// copies of the Software, and to permit persons to whom the Software is
/// furnished to do so, subject to the following conditions:
/// 
/// The above copyright notice and this permission notice shall be included in
/// all copies or substantial portions of the Software.
/// 
/// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
/// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
/// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
/// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
/// /// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
/// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
/// THE SOFTWARE.
/// 

namespace Almirante.Entities.Systems.Multithreaded
{
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Almirante.Entities.Filters;

    /// <summary>
    /// Parallel entity processor class.
    /// </summary>
    public abstract class ParallelEntityProcessor : EntityProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ParallelEntityProcessor"/> class.
        /// </summary>
        /// <param name="filter">The filter.</param>
        public ParallelEntityProcessor(Filter filter)
            : base(filter)
        {
        }

        /// <summary>
        /// Executes the current system.
        /// </summary>
        protected override void OnExecute(double time)
        {
            Parallel.ForEach(this.entities, (pair) =>
            {
                if (!pair.Value.Dead)
                {
                    this.Process(pair.Value);
                }
            });
        }
    }
}