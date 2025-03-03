#include <stdio.h>
#include <iostream>
#include <string>
#include <vector>
#include "pico/stdlib.h"

#include <serialLib.hpp>


int main()
{
    stdio_init_all();


    std::string buffer_str = "";


    while (true)
    {
        int gc = getchar_timeout_us(0);

        if (gc != PICO_ERROR_TIMEOUT)
        {
            if (gc == '\n' || gc == '\r')
            {
                if(!buffer_str.empty())
                {
                    CheckMessage(buffer_str);

                    buffer_str = "";
                }
            }
            else
            {
                buffer_str.push_back(gc);
            }
        }
    }

    return 0;
}

