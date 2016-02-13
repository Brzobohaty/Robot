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
        private Dictionary<string, string> dictionary = new Dictionary<string, string>(); //slovník

        private EposErrorCode(){
            dictionary.Add("05030000", "Toggle bit not alternated");
            dictionary.Add("05040000", "SDO protocol timed out");
            dictionary.Add("05040001", "Client /server command specifier not valid or unknown");
            dictionary.Add("05040002", "Invalid block size (block mode only)");
            dictionary.Add("05040003", "Invalid sequence number (block mode only)");
            dictionary.Add("05040004", "CRC error (block mode only)");
            dictionary.Add("05040005", "Out of Memory");
            dictionary.Add("06010000", "Unsupported access to an object (e.g. write command to a read-only object)");
            dictionary.Add("06010001", "Read command to a write only object");
            dictionary.Add("06010002", "Write command to a read only object");
            dictionary.Add("06020000", "Last read or write command had a wrong object index or subindex");
            dictionary.Add("06040041", "Object cannot be mapped to PDO");
            dictionary.Add("06040042", "Number and length of objects to be mapped would exceed PDO length");
            dictionary.Add("06040043", "General parameter incompatibility");
            dictionary.Add("06040047", "General internal incompatibility in device");
            dictionary.Add("06060000", "Access failed due to an hardware error");
            dictionary.Add("06070010", "Data type does not match, length or service parameter does not match");
            dictionary.Add("06070012", "Data type does not match, length or service parameter too high");
            dictionary.Add("06070013", "Data type does not match, length or service parameter too low");
            dictionary.Add("06090011", "Last read or write command had a wrong subindex");
            dictionary.Add("06090030", "Value range of parameter exceeded");
            dictionary.Add("06090031", "Value of parameter written too high");
            dictionary.Add("06090032", "Value of parameter written too low");
            dictionary.Add("06090036", "Maximum value is less than minimum value");
            dictionary.Add("08000000", "General error");
            dictionary.Add("08000020", "Data cannot be transferred or stored");
            dictionary.Add("08000021", "Data cannot be transferred or stored to application because of local control");
            dictionary.Add("08000022", "Data cannot be transferred or stored to application because of present device state");
            dictionary.Add("0F00FFB9", "Wrong CAN id");
            dictionary.Add("0F00FFBC", "Device is not in service mode");
            dictionary.Add("0F00FFBE", "Password is wrong");
            dictionary.Add("0F00FFBF", "RS232 command is illegal (does not exist)");
            dictionary.Add("0F00FFC0", "Device is in wrong NMT state");
            dictionary.Add("10000001", "Internal error");
            dictionary.Add("10000002", "Null pointer passed to function");
            dictionary.Add("10000003", "Handle passed to function is not valid");
            dictionary.Add("10000004", "Virtual device name is not valid");
            dictionary.Add("10000005", "Device name is not valid");
            dictionary.Add("10000006", "Protocol stack name is not valid");
            dictionary.Add("10000007", "Interface name is not valid");
            dictionary.Add("10000008", "Port is not valid");
            dictionary.Add("10000009", "Could not load external library");
            dictionary.Add("1000000A", "Command failed");
            dictionary.Add("1000000B", "Timeout occurred during execution");
            dictionary.Add("1000000C", "Bad parameter passed to function");
            dictionary.Add("1000000D", "Command aborted by user");
            dictionary.Add("1000000E", "Buffer is too small");
            dictionary.Add("1000000F", "No communication settings found");
            dictionary.Add("10000010", "Function not supported");
            dictionary.Add("10000011", "Parameter already used");
            dictionary.Add("10000020", "Bad device state");
            dictionary.Add("10000021", "Bad file content");
            dictionary.Add("10000022", "System cannot find specified path");
            dictionary.Add("20000001", "Error opening interface");
            dictionary.Add("20000002", "Error closing interface");
            dictionary.Add("20000003", "Interface is not open");
            dictionary.Add("20000004", "Error opening port");
            dictionary.Add("20000005", "Error closing port");
            dictionary.Add("20000006", "Port is not open");
            dictionary.Add("20000007", "Error resetting port");
            dictionary.Add("20000008", "Error configuring port settings");
            dictionary.Add("20000009", "Error configuring port mode");
            dictionary.Add("21000001", "Error writing data");
            dictionary.Add("21000002", "Error reading data");
            dictionary.Add("22000001", "Error receiving CAN frame");
            dictionary.Add("22000002", "Error transmitting CAN frame");
            dictionary.Add("23000001", "Error writing data");
            dictionary.Add("23000002", "Error reading data");
            dictionary.Add("31000001", "Negative acknowledge received");
            dictionary.Add("31000002", "Bad checksum received");
            dictionary.Add("31000003", "Bad data size received");
            dictionary.Add("32000001", "CAN frame of SDO protocol not received");
            dictionary.Add("32000002", "Requested CAN frame not received");
            dictionary.Add("32000003", "Can frame not received");
            dictionary.Add("34000001", "Failed stuffing data");
            dictionary.Add("34000002", "Failed destuffing data");
            dictionary.Add("34000003", "Bad CRC received");
            dictionary.Add("34000004", "Bad data received");
            dictionary.Add("34000005", "Bad data size written");
            dictionary.Add("34000006", "Failed writing data");
            dictionary.Add("34000007", "Failed reading data");
        }

        public static EposErrorCode getInstance() {
            return instanc;
        }
        
        /// <summary>
        /// Vrátí zprávu chyby daného kódu
        /// </summary>
        /// <param name="code">kód chyby</param>
        /// <returns>zpráva chyby</returns>
        public string getErrorMessage(uint code){
            return dictionary[String.Format("{0:X8}", code)];
        }
    }
}
