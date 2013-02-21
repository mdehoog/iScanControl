using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Globalization;
using Qixle.iScanDuo.Controller.Protocol;

namespace Qixle.iScanDuo.Controller.Serial
{
    public enum ErrorCode
    {
        NoError = 0,
        //duo errors
        InvalidCheckSum = 1,
        InvalidPacketId = 2,
        InvalidSettingId = 3,
        RangeError = 4,
        BadPacketCharacter = 5,
        SlowDataRate = 6,
        UnterminatedValue = 7,
        BadData = 8,
        BadDataLength = 9,
        ReadOnly = 10,
        OversizedPacket = 11,
        //communication errors
        Timeout = 12,
        InvalidResponseChecksum = 13,
        UnknownResponse = 14,
        NotConnected = 15,
        UnknownError = 16
    }

    public class DuoProtocol
    {
        private static ASCIIEncoding encoding = new ASCIIEncoding();

        public static byte[] CommandPacket<T>(Command<T> command, T value)
        {
            //prefix is for two/three parameter commands
            string prefix = command.HasValuePrefix ? (command.ValuePrefix + " ") : "";
            string commandString = command.Id + "\0" + prefix + command.ValueToString(value) + "\0";
            string complete = "\x02" + "30" + commandString.Length.ToString("X2") + commandString;
            string checksum = CalculateChecksum(complete);
            complete += checksum + "\x03";
            return encoding.GetBytes(complete);
        }

        public static byte[] QueryPacket<T>(Command<T> command)
        {
            //suffix is for two/three parameter commands
            string suffix = command.HasValuePrefix ? (command.ValuePrefix + "\0") : "";
            string query = command.Id + "\0" + suffix;
            string complete = "\x02" + "20" + query.Length.ToString("X2") + query;
            string checksum = CalculateChecksum(complete);
            complete += checksum + "\x03";
            return encoding.GetBytes(complete);
        }

        public static string CalculateChecksum(string s)
        {
            int total = 0;
            for (int i = 0; i < s.Length; i++)
                total += s[i];
            string totalString = total.ToString("X2");
            return totalString.Substring(totalString.Length - 2);
        }

        public static ErrorCode ReadResponse(SerialPort port, out string commandId, out string response)
        {
            response = null;
            commandId = null;
            try
            {
                int readCount = 0;
                byte[] header = new byte[5];
                while (readCount < header.Length)
                {
                    readCount += port.Read(header, readCount, header.Length - readCount);
                }

                if (header[0] != '\x02')
                    return ErrorCode.UnknownResponse;

                string headerString = encoding.GetString(header);
                string typeString = headerString.Substring(1, 2);
                string dataLengthString = headerString.Substring(3, 2);

                int dataLength;
                if (!int.TryParse(dataLengthString, NumberStyles.HexNumber, null, out dataLength))
                    return ErrorCode.UnknownResponse;

                readCount = 0;
                byte[] buffer = new byte[dataLength + 3]; //3 = checksum + ETX
                while (readCount < buffer.Length)
                {
                    readCount += port.Read(buffer, readCount, buffer.Length - readCount);
                }

                if (buffer[buffer.Length - 1] != '\x03')
                    return ErrorCode.UnknownResponse;

                string bufferString = encoding.GetString(buffer);
                string checksumString = bufferString.Substring(dataLength, 2);
                string calculatedChecksum = CalculateChecksum(headerString + bufferString.Substring(0, dataLength));
                if (!checksumString.Equals(calculatedChecksum))
                    return ErrorCode.InvalidResponseChecksum;

                if (typeString == "02") //error
                {
                    string errorString = bufferString.Substring(0, dataLength - 1);
                    int error;
                    if (!int.TryParse(errorString, out error))
                        return ErrorCode.UnknownResponse;

                    return (ErrorCode)error;
                }
                else if (typeString == "01") //response
                {
                    commandId = bufferString.Substring(0, 1);
                    response = bufferString.Substring(2, 2);

                    if (commandId != "1" || response != "30")
                        return ErrorCode.UnknownResponse;
                }
                else if (typeString == "21") //reply
                {
                    commandId = bufferString.Substring(0, 2);
                    response = bufferString.Substring(3, dataLength - 4);
                }
                else
                {
                    return ErrorCode.UnknownResponse;
                }

                return ErrorCode.NoError;
            }
            catch (TimeoutException)
            {
                return ErrorCode.Timeout;
            }
            catch (Exception)
            {
                return ErrorCode.UnknownResponse;
            }
            finally
            {
                if (port.IsOpen)
                {
                    port.DiscardInBuffer();
                }
            }
        }

        public static string TranslateQueryErrorCode(ICommand command, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.InvalidPacketId)
            {
                if (command == DuoCommands.InputSelectCommand)
                {
                    //querying InputSelect when InputSelect == 'auto' returns an InvalidPacketId error
                    return "0";
                }
                else if (command == DuoCommands.InputARPresetsCommand)
                {
                    //querying InputARPresents and AR preset is not currently enabled returns an InvalidPacketId error
                    return "5";
                }
            }
            return null;
        }

        public static ErrorCode TranslateSetErrorCode<T>(Command<T> command, T value, ErrorCode errorCode)
        {
            if (errorCode == ErrorCode.InvalidPacketId)
            {
                if ((ICommand)command == DuoCommands.DeinterlacerModeCommand)
                {
                    String valueString = command.ValueToString(value);
                    if ("6".Equals(valueString) || "1".Equals(valueString))
                    {
                        //setting DeinterlacerMode to 'Auto' or 'Video' results in InvalidPacketId
                        return ErrorCode.NoError;
                    }
                }
            }
            return errorCode;
        }
    }
}
