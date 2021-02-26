using System;
using System.Globalization;

namespace iSpyApplication.Pelco
{
    public class PelcoD
    {
        public enum Action
        {
            Start,
            Stop
        }

        [Flags]
        public enum Focus
        {
            Near = FocusNear,
            Far = FocusFar
        }

        [Flags]
        public enum Iris
        {
            Open = IrisOpen,
            Close = IrisClose
        }

        [Flags]
        public enum Pan
        {
            Left = PanLeft,
            Right = PanRight
        }

        public enum PatternAction
        {
            Start,
            Stop,
            Run
        }


        public enum PresetAction
        {
            Set,
            Clear,
            Goto
        }

        public enum Switch
        {
            On = 0x01,
            Off = 0x02
        }

        [Flags]
        public enum Tilt
        {
            Up = TiltUp,
            Down = TiltDown
        }

        [Flags]
        public enum Zoom
        {
            Wide = ZoomWide,
            Telephoto = ZoomTelephoto
        }

        private const byte Stx = 0xFF;

        private const byte FocusNear = 0x01;
        private const byte IrisOpen = 0x02;
        private const byte IrisClose = 0x04;
        private const byte CameraOnOff = 0x08;
        private const byte Sense = 0x80;

        private const byte PanRight = 0x02;
        private const byte PanLeft = 0x04;
        private const byte TiltUp = 0x08;
        private const byte TiltDown = 0x10;
        private const byte ZoomTelephoto = 0x20;
        private const byte ZoomWide = 0x40;
        private const byte FocusFar = 0x80;

        private const byte PanSpeedMax = 0xFF;
        private const byte TiltSpeedMax = 0x3F;

        public static byte[] Preset(uint deviceAddress, byte preset, PresetAction action)
        {
            byte mAction;
            switch (action)
            {
                case PresetAction.Set:
                    mAction = 0x03;
                    break;
                case PresetAction.Clear:
                    mAction = 0x05;
                    break;
                case PresetAction.Goto:
                    mAction = 0x07;
                    break;
                default:
                    mAction = 0x03;
                    break;
            }

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, preset);
        }

        public static byte[] Flip(uint deviceAddress) => Message.GetMessage(deviceAddress, 0x00, 0x07, 0x00, 0x21);

        public static byte[] ZeroPanPosition(uint deviceAddress) => Message.GetMessage(deviceAddress, 0x00, 0x07, 0x00, 0x22);


        public static byte[] RemoteReset(uint deviceAddress) => Message.GetMessage(deviceAddress, 0x00, 0x0F, 0x00, 0x00);

        public static byte[] Zone(uint deviceAddress, byte zone, Action action)
        {
            if ((zone < 0x01) & (zone > 0x08))
                throw new Exception("Zone should be between 0x01 and 0x08");
            var mAction = action == Action.Start ? (byte)0x11 : (byte)0x13;

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, zone);
        }

        public static byte[] ClearScreen(uint deviceAddress) => Message.GetMessage(deviceAddress, 0x00, 0x17, 0x00, 0x00);

        public static byte[] ZoneScan(uint deviceAddress, Action action)
        {
            var mAction = action == Action.Start ? (byte)0x1B : (byte)0x1D;
            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, 0x00);
        }

        public static byte[] Pattern(uint deviceAddress, PatternAction action)
        {
            byte mAction;
            switch (action)
            {
                case PatternAction.Start:
                    mAction = 0x1F;
                    break;
                case PatternAction.Stop:
                    mAction = 0x21;
                    break;
                case PatternAction.Run:
                    mAction = 0x23;
                    break;
                default:
                    mAction = 0x23;
                    break;
            }

            return Message.GetMessage(deviceAddress, 0x00, mAction, 0x00, 0x00);
        }

        public struct Message
        {
            public static byte Address;
            public static byte CheckSum;
            public static byte Command1, Command2, Data1, Data2;

            public static byte[] GetMessage(uint address, byte command1, byte command2, byte data1, byte data2)
            {
                if ((address < 1) & (address > 256))
                    throw new Exception("Pelco D protocol supports 256 devices only");

                Address = byte.Parse(address.ToString(CultureInfo.InvariantCulture));
                Data1 = data1;
                Data2 = data2;
                Command1 = command1;
                Command2 = command2;

                CheckSum = (byte)((Address + Command1 + Command2 + Data1 + Data2) % 256);


                return new[] { Stx, Address, Command1, Command2, Data1, Data2, CheckSum };
            }
        }


        #region Base Command Set

        public static byte[] CameraSwitch(uint deviceAddress, Switch action)
        {
            var mAction = CameraOnOff;
            if (action == Switch.On)
                mAction = CameraOnOff + Sense;
            return Message.GetMessage(deviceAddress, mAction, 0x00, 0x00, 0x00);
        }

        public static byte[] CameraIrisSwitch(uint deviceAddress, Iris action) => Message.GetMessage(deviceAddress, (byte)action, 0x00, 0x00, 0x00);

        public static byte[] CameraFocus(uint deviceAddress, Focus action) => action == Focus.Near
                ? Message.GetMessage(deviceAddress, (byte)action, 0x00, 0x00, 0x00)
                : Message.GetMessage(deviceAddress, 0x00, (byte)action, 0x00, 0x00);

        public static byte[] CameraZoom(uint deviceAddress, Zoom action) => Message.GetMessage(deviceAddress, 0x00, (byte)action, 0x00, 0x00);

        public static byte[] CameraTilt(uint deviceAddress, Tilt action, uint speed)
        {
            if (speed < TiltSpeedMax)
                speed = TiltSpeedMax;

            return Message.GetMessage(deviceAddress, 0x00, (byte)action, 0x00, (byte)speed);
        }

        public static byte[] CameraPan(uint deviceAddress, Pan action, uint speed)
        {
            if (speed < PanSpeedMax)
                speed = PanSpeedMax;

            return Message.GetMessage(deviceAddress, 0x00, (byte)action, (byte)speed, 0x00);
        }

        public static byte[] CameraStop(uint deviceAddress) => Message.GetMessage(deviceAddress, 0x00, 0x00, 0x00, 0x00);

        #endregion
    }
}