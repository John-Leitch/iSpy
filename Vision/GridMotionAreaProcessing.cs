﻿// AForge Vision Library
// AForge.NET framework
// http://www.aforgenet.com/framework/
//
// Copyright © AForge.NET, 2005-2011
// contacts@aforgenet.com
//

using AForge.Imaging;
using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace iSpyApplication.Vision
{
    /// <summary>
    /// Motion processing algorithm, which performs grid processing of motion frame.
    /// </summary>
    /// 
    /// <remarks><para>The aim of this motion processing algorithm is to do grid processing
    /// of motion frame. This means that entire motion frame is divided by a grid into
    /// certain amount of cells and the motion level is calculated for each cell. The
    /// information about each cell's motion level may be retrieved using <see cref="MotionGrid"/>
    /// property.</para>
    /// 
    /// <para><para>In addition the algorithm can highlight those cells, which have motion
    /// level above the specified threshold (see <see cref="MotionAmountToHighlight"/>
    /// property). To enable this it is required to set <see cref="HighlightMotionGrid"/>
    /// property to <see langword="true"/>.</para></para>
    /// 
    /// <para>Sample usage:</para>
    /// <code>
    /// // create instance of motion detection algorithm
    /// IMotionDetector motionDetector = new ... ;
    /// // create instance of motion processing algorithm
    /// GridMotionAreaProcessing motionProcessing = new GridMotionAreaProcessing( 16, 16 );
    /// // create motion detector
    /// MotionDetector detector = new MotionDetector( motionDetector, motionProcessing );
    /// 
    /// // continuously feed video frames to motion detector
    /// while ( ... )
    /// {
    ///     // process new video frame
    ///     detector.ProcessFrame( videoFrame );
    ///     
    ///     // check motion level in 5th row 8th column
    ///     if ( motionProcessing.MotionGrid[5, 8] > 0.15 )
    ///     {
    ///         // ...
    ///     }
    /// }
    /// </code>
    /// </remarks>
    /// 
    /// <seealso cref="MotionDetector"/>
    /// <seealso cref="IMotionDetector"/>
    /// 
    public class GridMotionAreaProcessing : IMotionProcessing
    {
        // color used for highlighting motion grid
        private Color _highlightColor = Color.Red;
        // highlight motion grid or not
        private bool _highlightMotionGrid;

        private float _motionAmountToHighlight;

        private int _gridWidth;
        private int _gridHeight;

        private float[,] _motionGrid;

        /// <summary>
        /// Color used to highlight motion regions.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>Default value is set to <b>red</b> color.</para>
        /// </remarks>
        /// 
        public Color HighlightColor
        {
            get => _highlightColor;
            set => _highlightColor = value;
        }

        /// <summary>
        /// Highlight motion regions or not.
        /// </summary>
        /// 
        /// <remarks><para>The property specifies if motion grid should be highlighted -
        /// if cell, which have motion level above the
        /// <see cref="MotionAmountToHighlight">specified value</see>, should be highlighted.</para>
        /// 
        /// <para>Default value is set to <see langword="true"/>.</para>
        ///
        /// <para><note>Turning the value on leads to additional processing time of video frame.</note></para>
        /// </remarks>
        /// 
        public bool HighlightMotionGrid
        {
            get => _highlightMotionGrid;
            set => _highlightMotionGrid = value;
        }

        /// <summary>
        /// Motion amount to highlight cell.
        /// </summary>
        /// 
        /// <remarks><para>The property specifies motion level threshold for highlighting grid's
        /// cells. If motion level of a certain cell is higher than this value, then the cell
        /// is highlighted.</para>
        /// 
        /// <para>Default value is set to <b>0.15</b>.</para>
        /// </remarks>
        /// 
        public float MotionAmountToHighlight
        {
            get => _motionAmountToHighlight;
            set => _motionAmountToHighlight = value;
        }

        /// <summary>
        /// Motion levels of each grid's cell.
        /// </summary>
        /// 
        /// <remarks><para>The property represents an array of size
        /// <see cref="GridHeight"/>x<see cref="GridWidth"/>, which keeps motion level
        /// of each grid's cell. If certain cell has motion level equal to 0.2, then it
        /// means that this cell has 20% of changes.</para>
        /// </remarks>
        /// 
        public float[,] MotionGrid => _motionGrid;

        /// <summary>
        /// Width of motion grid, [2, 64].
        /// </summary>
        /// 
        /// <remarks><para>The property specifies motion grid's width - number of grid' columns.</para>
        ///
        /// <para>Default value is set to <b>16</b>.</para>
        /// </remarks>
        /// 
        public int GridWidth
        {
            get => _gridWidth;
            set
            {
                _gridWidth = Math.Min(64, Math.Max(2, value));
                _motionGrid = new float[_gridHeight, _gridWidth];
            }
        }

        /// <summary>
        /// Height of motion grid, [2, 64].
        /// </summary>
        /// 
        /// <remarks><para>The property specifies motion grid's height - number of grid' rows.</para>
        ///
        /// <para>Default value is set to <b>16</b>.</para>
        /// </remarks>
        /// 
        public int GridHeight
        {
            get => _gridHeight;
            set
            {
                _gridHeight = Math.Min(64, Math.Max(2, value));
                _motionGrid = new float[_gridHeight, _gridWidth];
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMotionAreaProcessing"/> class.
        /// </summary>
        /// 
        public GridMotionAreaProcessing() : this(16, 16) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMotionAreaProcessing"/> class.
        /// </summary>
        /// 
        /// <param name="gridWidth">Width of motion grid (see <see cref="GridWidth"/> property).</param>
        /// <param name="gridHeight">Height of motion grid (see <see cref="GridHeight"/> property).</param>
        /// 
        public GridMotionAreaProcessing(int gridWidth, int gridHeight) : this(gridWidth, gridHeight, true) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMotionAreaProcessing"/> class.
        /// </summary>
        /// 
        /// <param name="gridWidth">Width of motion grid (see <see cref="GridWidth"/> property).</param>
        /// <param name="gridHeight">Height of motion grid (see <see cref="GridHeight"/> property).</param>
        /// <param name="highlightMotionGrid">Highlight motion regions or not (see <see cref="HighlightMotionGrid"/> property).</param>
        ///
        public GridMotionAreaProcessing(int gridWidth, int gridHeight, bool highlightMotionGrid)
            : this(gridWidth, gridHeight, highlightMotionGrid, 0.15f) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="GridMotionAreaProcessing"/> class.
        /// </summary>
        /// 
        /// <param name="gridWidth">Width of motion grid (see <see cref="GridWidth"/> property).</param>
        /// <param name="gridHeight">Height of motion grid (see <see cref="GridHeight"/> property).</param>
        /// <param name="highlightMotionGrid">Highlight motion regions or not (see <see cref="HighlightMotionGrid"/> property).</param>
        /// <param name="motionAmountToHighlight">Motion amount to highlight cell (see <see cref="MotionAmountToHighlight"/> property).</param>
        ///
        public GridMotionAreaProcessing(int gridWidth, int gridHeight, bool highlightMotionGrid, float motionAmountToHighlight)
        {
            _gridWidth = Math.Min(64, Math.Max(2, gridWidth));
            _gridHeight = Math.Min(64, Math.Max(2, gridHeight));

            _motionGrid = new float[gridHeight, gridWidth];

            _highlightMotionGrid = highlightMotionGrid;
            _motionAmountToHighlight = motionAmountToHighlight;
        }

        /// <summary>
        /// Process video and motion frames doing further post processing after
        /// performed motion detection.
        /// </summary>
        /// 
        /// <param name="videoFrame">Original video frame.</param>
        /// <param name="motionFrame">Motion frame provided by motion detection
        /// algorithm (see <see cref="IMotionDetector"/>).</param>
        /// 
        /// <remarks><para>Processes provided motion frame and calculates motion level
        /// for each grid's cell. In the case if <see cref="HighlightMotionGrid"/> property is
        /// set to <see langword="true"/>, the cell with motion level above threshold are
        /// highlighted.</para></remarks>
        ///
        /// <exception cref="InvalidImagePropertiesException">Motion frame is not 8 bpp image, but it must be so.</exception>
        /// <exception cref="UnsupportedImageFormatException">Video frame must be 8 bpp grayscale image or 24/32 bpp color image.</exception>
        ///
        public unsafe void ProcessFrame(UnmanagedImage videoFrame, UnmanagedImage motionFrame)
        {
            if (motionFrame.PixelFormat != PixelFormat.Format8bppIndexed)
            {
                throw new InvalidImagePropertiesException("Motion frame must be 8 bpp image.");
            }

            if ((videoFrame.PixelFormat != PixelFormat.Format8bppIndexed) &&
                 (videoFrame.PixelFormat != PixelFormat.Format24bppRgb) &&
                 (videoFrame.PixelFormat != PixelFormat.Format32bppRgb) &&
                 (videoFrame.PixelFormat != PixelFormat.Format32bppArgb))
            {
                throw new UnsupportedImageFormatException("Video frame must be 8 bpp grayscale image or 24/32 bpp color image.");
            }

            int width = videoFrame.Width;
            int height = videoFrame.Height;
            int pixelSize = System.Drawing.Image.GetPixelFormatSize(videoFrame.PixelFormat) / 8;

            if ((motionFrame.Width != width) || (motionFrame.Height != height))
                return;

            int cellWidth = width / _gridWidth;
            int cellHeight = height / _gridHeight;

            // temporary variables
            int xCell, yCell;

            // process motion frame calculating amount of changed pixels
            // in each grid's cell
            byte* motion = (byte*)motionFrame.ImageData.ToPointer();
            int motionOffset = motionFrame.Stride - width;

            for (int y = 0; y < height; y++)
            {
                // get current grid's row
                yCell = y / cellHeight;
                // correct row number if image was not divided by grid equally
                if (yCell >= _gridHeight)
                    yCell = _gridHeight - 1;

                for (int x = 0; x < width; x++, motion++)
                {
                    if (*motion != 0)
                    {
                        // get current grid's collumn
                        xCell = x / cellWidth;
                        // correct column number if image was not divided by grid equally
                        if (xCell >= _gridWidth)
                            xCell = _gridWidth - 1;

                        _motionGrid[yCell, xCell]++;
                    }
                }
                motion += motionOffset;
            }

            // update motion grid converting absolute number of changed
            // pixel to relative for each cell
            int gridHeightM1 = _gridHeight - 1;
            int gridWidthM1 = _gridWidth - 1;

            int lastRowHeight = height - cellHeight * gridHeightM1;
            int lastColumnWidth = width - cellWidth * gridWidthM1;

            for (int y = 0; y < _gridHeight; y++)
            {
                int ch = (y != gridHeightM1) ? cellHeight : lastRowHeight;

                for (int x = 0; x < _gridWidth; x++)
                {
                    int cw = (x != gridWidthM1) ? cellWidth : lastColumnWidth;

                    _motionGrid[y, x] /= (cw * ch);
                }
            }

            if (_highlightMotionGrid)
            {
                // highlight motion grid - cells, which have enough motion

                byte* src = (byte*)videoFrame.ImageData.ToPointer();
                int srcOffset = videoFrame.Stride - width * pixelSize;

                if (pixelSize == 1)
                {
                    // grayscale case
                    byte fillG = (byte)(0.2125 * _highlightColor.R +
                                          0.7154 * _highlightColor.G +
                                          0.0721 * _highlightColor.B);

                    for (int y = 0; y < height; y++)
                    {
                        yCell = y / cellHeight;
                        if (yCell >= _gridHeight)
                            yCell = _gridHeight - 1;

                        for (int x = 0; x < width; x++, src++)
                        {
                            xCell = x / cellWidth;
                            if (xCell >= _gridWidth)
                                xCell = _gridWidth - 1;

                            if ((_motionGrid[yCell, xCell] > _motionAmountToHighlight) && (((x + y) & 1) == 0))
                            {
                                *src = fillG;
                            }
                        }
                        src += srcOffset;
                    }
                }
                else
                {
                    // color case
                    byte fillR = _highlightColor.R;
                    byte fillG = _highlightColor.G;
                    byte fillB = _highlightColor.B;

                    for (int y = 0; y < height; y++)
                    {
                        yCell = y / cellHeight;
                        if (yCell >= _gridHeight)
                            yCell = _gridHeight - 1;

                        for (int x = 0; x < width; x++, src += pixelSize)
                        {
                            xCell = x / cellWidth;
                            if (xCell >= _gridWidth)
                                xCell = _gridWidth - 1;

                            if ((_motionGrid[yCell, xCell] > _motionAmountToHighlight) && (((x + y) & 1) == 0))
                            {
                                src[RGB.R] = fillR;
                                src[RGB.G] = fillG;
                                src[RGB.B] = fillB;
                            }
                        }
                        src += srcOffset;
                    }
                }
            }
        }

        /// <summary>
        /// Reset internal state of motion processing algorithm.
        /// </summary>
        /// 
        /// <remarks><para>The method allows to reset internal state of motion processing
        /// algorithm and prepare it for processing of next video stream or to restart
        /// the algorithm.</para></remarks>
        ///
        public void Reset()
        {
        }
    }
}
