#include <stdio.h>

/* 
   simple code to test and use analog  3 pin  4x4 keypad 
    
    2021 ~ by rogermarin
    this code is in public domain
    thsi program uses the serial monitor to see tue values for each button 
    because the hrdware di  not 100% reliable on the readings I gave a + - 5 ohms of tolerance to all the mesurements 

    Modified to minimize the chances of outliers appearing when pressing a button
    2022 ~ modified by Nelson Saraiva (@devNelson)
*/
int analogPin = A0; // Or any pin you are using 

int sampleVals[6];  // Array that stores a sample of the values generated by the pressed button. Now are 6.
int arrElementCount = 0; // Count the number of values in the sample

int val = 0;  // an auxiliar variable to store  the value
int valToSend = 0; // This variable is the value of the pressed button sent to serial

void setup () 
{
 Serial.begin (9600); //Setup serial to use the serial monitor 
}

int getMaxRepeatingElement(int array[], int n) 
{
  int i, j, maxElement, count;
     int maxCount = 0;
    /* Frequency of each element is counted and checked.If it's greater than the utmost count element we found till now, then it is updated accordingly  */  
    for(i = 0; i< n; i++)   //For loop to hold each element
    {
        count = 1;
        for(j = i+1; j < n; j++)  //For loop to check for duplicate elements
        {
            if(array[j] == array[i])
            {
                count++;     //Increment  count
                /* If count of current element is more than 
                maxCount, then update maxElement */
                if(count > maxCount)
                {
                    maxElement = array[j];
                }  
            }
        }
    }
    return maxElement;
}

void loop () 
{
  val = analogRead (analogPin);

  // Taking a sample of the values generated by pressing the button
  if(val > 5){
    if(arrElementCount < 6){
    sampleVals[arrElementCount] = val;
    arrElementCount++;      
    }
  }

  if(arrElementCount > 5 && val < 5){
    valToSend = getMaxRepeatingElement(sampleVals, 6);  // Get the value that appears most in the sample
    
    if (valToSend >= 1000 and valToSend <= 1025){Serial.println ("button 1");};  //  Value by the manufacturer  1 - 1023 the real value could be very diferent 
    if (valToSend >= 917 and valToSend <= 932){Serial.println ("button 2");}; //value by the manufacturer 2- 930  the real value could be very diferent 
    if (valToSend >= 850 and valToSend <= 865){Serial.println ("button 3");};// value by the manufacturer 3 850 the real value could be very diferent 
    if (valToSend >= 780 and valToSend <= 795){Serial.println ("button 4");};// value by the manufacturer 4 790 the real value could be very diferent 
    if (valToSend >= 670 and valToSend <= 685){Serial.println ("button 5");};// value by the manufacturer 5 680 the real value could be very diferent 
    if (valToSend >= 630 and valToSend <= 645){Serial.println ("button 6");};// value by the manufacturer 6 640 the real value could be very diferent 
    if (valToSend >= 590 and valToSend <= 605){Serial.println ("button 7");};// value by the manufacturer 7 600 the real value could be very diferent 
    if (valToSend >= 550 and valToSend <= 565){Serial.println ("button 8");};// value by the manufacturer 8 570 the real value could be very diferent 
    if (valToSend >= 490 and valToSend <= 505){Serial.println ("button 9");};// value by the manufacturer 9 512 the real value could be very diferent 
    if (valToSend >= 470 and valToSend <= 485){Serial.println ("button 10");};// value by the manufacturer 10 487 the real value could be very diferent 
    if (valToSend >= 450 and valToSend <= 465){Serial.println ("button 11");};// value by the manufacturer 11 465 the real value could be very diferent 
    if (valToSend >= 435 and valToSend <= 450){Serial.println ("button 12");};// value by the manufacturer 12 445 the real value could be very diferent 
    if (valToSend >= 390 and valToSend <= 405){Serial.println ("button 13");};// value by the manufacturer 13 410 the real value could be very diferent 
    if (valToSend >= 310 and valToSend <= 325){Serial.println ("button 14");};// value by the manufacturer 14 330 the real value could be very diferent 
    if (valToSend >= 255 and valToSend <= 270){Serial.println ("button 15");};// value by the manufacturer 15 277 the real value could be very diferent 
    if (valToSend >= 220 and valToSend <= 235){Serial.println ("button 16");};// value by the manufacturer 16 238 the real value could be very diferent 
      
    valToSend = 0;
    arrElementCount = 0;
  }
}
