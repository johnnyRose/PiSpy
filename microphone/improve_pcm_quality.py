f = open('a.pcm', 'rb')

array = []
array += f.read()

def cut(array, big, small):
    # caps the biggest and smallest values
    pivot = 0
    quiet = 128
    adjustment = pivot - quiet
    clipped_array = [max(min(val, big), small) for val in array]
    for i in range(len(clipped_array)):
        #print(clipped_array[i])
        # adjust for neutral value
        new_val = clipped_array[i] + adjustment
        # amplify
        new_val = new_val * 128 / (big - small)
        # back to unsigned
        clipped_array[i] = new_val + quiet
        if (clipped_array[i] > 255 or clipped_array[i] < 0):
            print(clipped_array[i])
    return clipped_array

def low_pass_filter(array):
    p_filter = [1/6, 2/3, 1/6]
    #p_filter = [1/12, 1/6, 1/2, 1/6, 1/12]
    f_len = len(p_filter)
    assert f_len % 2 == 1

    middle = int(f_len / 2) + 1

    filtered_array = [0] * len(array)
    for i in range(middle - 1, len(array) - (middle - 1)):
        total = 0
        for j in range(middle - f_len, f_len - middle + 1):
            total += p_filter[j + middle - 1] * array[i + j]
        filtered_array[i] = round(total)
    return filtered_array

diff = 30
middle = 128
array = cut(array, middle + diff, middle - diff)
# instead of low pass perhaps we should try median pass
array = low_pass_filter(array)
f2 = open('improved.raw', 'wb')
f2.write(bytearray(array))
f2.close()
