import struct, socket  
import hashlib  
import threading, random  
import time  
from base64 import b64encode, b64decode  
   
def hex2dec(string_num):  
    return str(int(string_num.upper(), 16))  

def get_datalength(msg):   
    code_length = ord(msg[1]) & 127  
    
    if code_length == 126:                 
        code_length = struct.unpack('>H', str(msg[2:4]))[0]  
        header_length = 8  
    elif code_length == 127:           
        code_length = struct.unpack('>Q', str(msg[2:10]))[0]  
        header_length = 14  
    else:  
        header_length = 6  
    code_length = int(code_length)  
    
    return (header_length, code_length)
          
def parse_data(msg):      
    code_length = ord(msg[1]) & 127  
    if code_length == 126:  
        code_length = struct.unpack('>H', str(msg[2:4]))[0]  
        masks = msg[4:8]  
        data = msg[8:]  
    elif code_length == 127:  
        code_length = struct.unpack('>Q', str(msg[2:10]))[0]  
        masks = msg[10:14]  
        data = msg[14:]  
    else:  
        masks = msg[2:6]  
        data = msg[6:]  
    
    i = 0  
    raw_str = ''  
    
    for d in data:  
        raw_str += chr(ord(d) ^ ord(masks[i % 4]))  
        i += 1  
       
    return raw_str    
  
  
def processMessage(message):         
    message_utf_8 = message.encode('utf-8')  
    back_str = []  
    back_str.append('\x81')  
    data_length = len(message_utf_8)    
    
    if data_length <= 125:  
        back_str.append(chr(data_length))  
    elif data_length <= 65535 :  
        back_str.append(struct.pack('b', 126))  
        back_str.append(struct.pack('>h', data_length))        
    elif data_length <= (2 ^ 64 - 1):  
        back_str.append(struct.pack('b', 127))  
        back_str.append(struct.pack('>q', data_length))          
    else :  
        print "too long"
               
    msg = ''  
    for c in back_str:  
        msg += c  
    back_str = str(msg) + message_utf_8  # .encode('utf-8')      
    
    if back_str != None and len(back_str) > 0:  
        return back_str
    else:
        return None
            


   
