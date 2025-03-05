#ifndef _SERIAL_LIB_
#define _SERIAL_LIB_

#include <stdio.h>
#include <string>
#include <vector>
#include <iostream>
#include <sstream>
#include "pico/stdlib.h"


enum RECEIVED_MODE
{
    NONE,
    SET_OUT,
    SET_IN,
    PUT,
};

RECEIVED_MODE _receive_mode;

std::string GetReceiveMode(){
    std::string str;

    switch (_receive_mode)
    {
    case NONE:
        str = "MODE:NONE";
        break;

    case SET_OUT:
        str = "MODE:SET_OUT";
        break;

    case SET_IN:
        str = "MODE:SET_IN";
        break;

    case PUT:
        str = "MODE:PUT";
        break;

    default:
        str = "MODE:ERROR";
        break;
    }


    return str;
}

void InitGpio(uint pin)
{
    gpio_init(pin);

    std::cout << "init_pin:" << pin << std::endl;
}

void SetInGpio(uint pin){
    InitGpio(pin);
    gpio_set_dir(pin,GPIO_IN);

    std::cout << "in_pin:" << pin << std::endl;
}

void SetOutPin(uint pin){
    InitGpio(pin);
    gpio_set_dir(pin,GPIO_OUT);

    std::cout << "out_pin:" << pin << std::endl;
}

void PutGpio(std::string message)
{
    int message_parsed[2];

    std::stringstream ss(message);

    std::string s;

    int i=0;

    while(std::getline(ss,s,',') && i < 2)
    {
        message_parsed[i] = atoi(s.c_str());
        i++;
    }

    std::cout << "pin_number:" << message_parsed[0] << "  put:" << message_parsed[1] << std::endl;

    gpio_put(message_parsed[0],message_parsed[1]);

}

void CheckMessage(std::string message)
{
    if(message[0] == '@')
    {
        if (message=="@RESET")
        {
            stdio_init_all();
            std::cout << "reset" << std::endl;
        }
        else if(message == "@GET_MODE")
        {
            std::cout << GetReceiveMode() << std::endl;
        }
        else if (message=="@SET_OUT")
        {
            _receive_mode = SET_OUT;
            std::cout << "out_mode" << std::endl;
        }
        else if (message=="@SET_IN")
        {
            _receive_mode = SET_IN;
            std::cout << "in_mode" << std::endl;
        }
        else if (message=="@PUT")
        {
            _receive_mode = PUT;
            std::cout << "put_mode" << std::endl;
        }
        else if(message=="@NONE")
        {
            _receive_mode = NONE;
            std::cout << "none" << std::endl;
        }
        else
        {
            _receive_mode = NONE;
            printf("ERROR:undefined mode\r\n");
        }
    }
    else
    {
        uint pin;
        switch (_receive_mode)
        {
        case SET_OUT:
            pin = atoi(message.c_str());
            SetOutPin(pin);
            break;

        case SET_IN:
            pin = atoi(message.c_str());
            SetInGpio(pin);
            break;
        case PUT:
            PutGpio(message);
            break;

        default:
            break;
        }
    }
}

#endif // !_SERIAL_LIB_
