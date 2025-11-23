#include "pico/stdlib.h"
#include "hardware/adc.h"
#include <stdio.h>
#include <string.h>

#define LED_PIN 15   // Change to your GPIO pin
#define SENSOR_ADC 0 // ADC0 on GP26
#define BUF_SIZE 32

int main()
{
    stdio_init_all(); // Initialize USB serial
    adc_init();
    adc_gpio_init(26); // GP26 = ADC0
    adc_select_input(SENSOR_ADC);

    gpio_init(LED_PIN);
    gpio_set_dir(LED_PIN, true);

    int sensor_value = 0;
    char buf[BUF_SIZE];

    while (true)
    {
        sensor_value = adc_read();
        printf("%d\n", sensor_value); // Send to PC

        if (stdio_usb_connected())
        {
            if (fgets(buf, BUF_SIZE, stdin)) {
                // Remove trailing newline
                buf[strcspn(buf, "\r\n")] = 0;

                if (strcmp(buf, "ledOn") == 0) {
                    gpio_put(LED_PIN, 1);
                } else if (strcmp(buf, "ledOff") == 0) {
                    gpio_put(LED_PIN, 0);
                }
            }
        }

        sleep_ms(2000); // Sample every 2 seconds
    }
}
