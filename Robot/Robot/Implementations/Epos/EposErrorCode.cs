using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Robot.Implementations.Epos
{
    /// <summary>
    /// Překladač error kódů EPOS motorů
    /// </summary>
    public class EposErrorCode
    {
        private static EposErrorCode instanc = new EposErrorCode();
        private Dictionary<string, string> dictionaryComumnication = new Dictionary<string, string>(); //slovník pro komunikační chyby
        private Dictionary<string, string> dictionaryDevice = new Dictionary<string, string>(); //slovník pro chyby zařízení

        private EposErrorCode(){
            dictionaryComumnication.Add("00000000", "No connection choosen");
            dictionaryComumnication.Add("05030000", "Toggle bit not alternated");
            dictionaryComumnication.Add("05040000", "SDO protocol timed out");
            dictionaryComumnication.Add("05040001", "Client /server command specifier not valid or unknown");
            dictionaryComumnication.Add("05040002", "Invalid block size (block mode only)");
            dictionaryComumnication.Add("05040003", "Invalid sequence number (block mode only)");
            dictionaryComumnication.Add("05040004", "CRC error (block mode only)");
            dictionaryComumnication.Add("05040005", "Out of Memory");
            dictionaryComumnication.Add("06010000", "Unsupported access to an object (e.g. write command to a read-only object)");
            dictionaryComumnication.Add("06010001", "Read command to a write only object");
            dictionaryComumnication.Add("06010002", "Write command to a read only object");
            dictionaryComumnication.Add("06020000", "Last read or write command had a wrong object index or subindex");
            dictionaryComumnication.Add("06040041", "Object cannot be mapped to PDO");
            dictionaryComumnication.Add("06040042", "Number and length of objects to be mapped would exceed PDO length");
            dictionaryComumnication.Add("06040043", "General parameter incompatibility");
            dictionaryComumnication.Add("06040047", "General internal incompatibility in device");
            dictionaryComumnication.Add("06060000", "Access failed due to an hardware error");
            dictionaryComumnication.Add("06070010", "Data type does not match, length or service parameter does not match");
            dictionaryComumnication.Add("06070012", "Data type does not match, length or service parameter too high");
            dictionaryComumnication.Add("06070013", "Data type does not match, length or service parameter too low");
            dictionaryComumnication.Add("06090011", "Last read or write command had a wrong subindex");
            dictionaryComumnication.Add("06090030", "Value range of parameter exceeded");
            dictionaryComumnication.Add("06090031", "Value of parameter written too high");
            dictionaryComumnication.Add("06090032", "Value of parameter written too low");
            dictionaryComumnication.Add("06090036", "Maximum value is less than minimum value");
            dictionaryComumnication.Add("08000000", "General error");
            dictionaryComumnication.Add("08000020", "Data cannot be transferred or stored");
            dictionaryComumnication.Add("08000021", "Data cannot be transferred or stored to application because of local control");
            dictionaryComumnication.Add("08000022", "Data cannot be transferred or stored to application because of present device state");
            dictionaryComumnication.Add("0F00FFB9", "Wrong CAN id");
            dictionaryComumnication.Add("0F00FFBC", "Device is not in service mode");
            dictionaryComumnication.Add("0F00FFBE", "Password is wrong");
            dictionaryComumnication.Add("0F00FFBF", "RS232 command is illegal (does not exist)");
            dictionaryComumnication.Add("0F00FFC0", "Device is in wrong NMT state");
            dictionaryComumnication.Add("10000001", "Internal error");
            dictionaryComumnication.Add("10000002", "Null pointer passed to function");
            dictionaryComumnication.Add("10000003", "Handle passed to function is not valid");
            dictionaryComumnication.Add("10000004", "Virtual device name is not valid");
            dictionaryComumnication.Add("10000005", "Device name is not valid");
            dictionaryComumnication.Add("10000006", "Protocol stack name is not valid");
            dictionaryComumnication.Add("10000007", "Interface name is not valid");
            dictionaryComumnication.Add("10000008", "Port is not valid");
            dictionaryComumnication.Add("10000009", "Could not load external library");
            dictionaryComumnication.Add("1000000A", "Command failed");
            dictionaryComumnication.Add("1000000B", "Timeout occurred during execution");
            dictionaryComumnication.Add("1000000C", "Bad parameter passed to function");
            dictionaryComumnication.Add("1000000D", "Command aborted by user");
            dictionaryComumnication.Add("1000000E", "Buffer is too small");
            dictionaryComumnication.Add("1000000F", "No communication settings found");
            dictionaryComumnication.Add("10000010", "Function not supported");
            dictionaryComumnication.Add("10000011", "Parameter already used");
            dictionaryComumnication.Add("10000020", "Bad device state");
            dictionaryComumnication.Add("10000021", "Bad file content");
            dictionaryComumnication.Add("10000022", "System cannot find specified path");
            dictionaryComumnication.Add("20000001", "Error opening interface");
            dictionaryComumnication.Add("20000002", "Error closing interface");
            dictionaryComumnication.Add("20000003", "Interface is not open");
            dictionaryComumnication.Add("20000004", "Error opening port");
            dictionaryComumnication.Add("20000005", "Error closing port");
            dictionaryComumnication.Add("20000006", "Port is not open");
            dictionaryComumnication.Add("20000007", "Error resetting port");
            dictionaryComumnication.Add("20000008", "Error configuring port settings");
            dictionaryComumnication.Add("20000009", "Error configuring port mode");
            dictionaryComumnication.Add("21000001", "Error writing data");
            dictionaryComumnication.Add("21000002", "Error reading data");
            dictionaryComumnication.Add("22000001", "Error receiving CAN frame");
            dictionaryComumnication.Add("22000002", "Error transmitting CAN frame");
            dictionaryComumnication.Add("23000001", "Error writing data");
            dictionaryComumnication.Add("23000002", "Error reading data");
            dictionaryComumnication.Add("31000001", "Negative acknowledge received");
            dictionaryComumnication.Add("31000002", "Bad checksum received");
            dictionaryComumnication.Add("31000003", "Bad data size received");
            dictionaryComumnication.Add("32000001", "CAN frame of SDO protocol not received");
            dictionaryComumnication.Add("32000002", "Requested CAN frame not received");
            dictionaryComumnication.Add("32000003", "Can frame not received");
            dictionaryComumnication.Add("34000001", "Failed stuffing data");
            dictionaryComumnication.Add("34000002", "Failed destuffing data");
            dictionaryComumnication.Add("34000003", "Bad CRC received");
            dictionaryComumnication.Add("34000004", "Bad data received");
            dictionaryComumnication.Add("34000005", "Bad data size written");
            dictionaryComumnication.Add("34000006", "Failed writing data");
            dictionaryComumnication.Add("34000007", "Failed reading data");

            dictionaryDevice.Add("0000", "No Error");
            dictionaryDevice.Add("1000", "Generic Error");
            dictionaryDevice.Add("2310", "Over Current Error");
            dictionaryDevice.Add("3210", "Over Voltage Error");
            dictionaryDevice.Add("3220", "Under Voltage");
            dictionaryDevice.Add("4210", "Over Temperature");
            dictionaryDevice.Add("5113", "Supply Voltage (+5V) too low");
            dictionaryDevice.Add("6100", "Internal Software Error");
            dictionaryDevice.Add("6320", "Software Parameter Error");
            dictionaryDevice.Add("7320", "Sensor Position Error");
            dictionaryDevice.Add("8110", "CAN Overrun Error (Objects lost)");
            dictionaryDevice.Add("8111", "CAN Overrun Error");
            dictionaryDevice.Add("8120", "CAN Passive Mode Error");
            dictionaryDevice.Add("8130", "CAN Life Guard Error");
            dictionaryDevice.Add("8150", "CAN Transmit COB-ID collision");
            dictionaryDevice.Add("81FD", "CAN Bus Off");
            dictionaryDevice.Add("81FE", "CAN Rx Queue Overrun");
            dictionaryDevice.Add("81FF", "CAN Tx Queue Overrun");
            dictionaryDevice.Add("8210", "CAN PDO length Error");
            dictionaryDevice.Add("8611", "Following Error");
            dictionaryDevice.Add("FF01", "Hall Sensor Error");
            dictionaryDevice.Add("FF02", "Index Processing Error");
            dictionaryDevice.Add("FF03", "Encoder Resolution Error");
            dictionaryDevice.Add("FF04", "Hallsensor not found Error");
            dictionaryDevice.Add("FF06", "Negative Limit Error");
            dictionaryDevice.Add("FF07", "Positive Limit Error");
            dictionaryDevice.Add("FF08", "Hall Angle detection Error");
            dictionaryDevice.Add("FF09", "Software Position Limit Error");
            dictionaryDevice.Add("FF0A", "Position Sensor Breach");
            dictionaryDevice.Add("FF0B", "System Overloaded");
        }

        public static EposErrorCode getInstance() {
            return instanc;
        }
        
        /// <summary>
        /// Vrátí zprávu chyby v komunikaci daného kódu
        /// </summary>
        /// <param name="code">kód chyby</param>
        /// <returns>zpráva chyby</returns>
        public string getComunicationErrorMessage(uint code){
            try{
                return dictionaryComumnication[String.Format("{0:X8}", code)];
            }catch(KeyNotFoundException){
                return "Neznámá chyba";
            }
        }

        /// <summary>
        /// Vrátí zprávu chyby řídící jednotky daného kódu
        /// </summary>
        /// <param name="code">kód chyby</param>
        /// <returns>zpráva chyby</returns>
        public string getDeviceErrorMessage(uint code)
        {
            try
            {
                return dictionaryDevice[String.Format("{0:X4}", code)];
            }
            catch (KeyNotFoundException)
            {
                return "Neznámá chyba";
            }
        }
    }
}
