1000萬筆資料

1. 批次寫入不同檔案 
2. 每一次執行完就dispose GC


實驗零：
無分批寫入
一萬: 0.1s
十萬: 1s
百萬: 6s
千萬: 240s


實驗一：
分批寫入，每個batch共1萬筆，共n/10000個batch，並且seriel的方式進行
一萬:0.142s
十萬:0.691s
百萬:7.886s
千萬:124.4s (batch size = 10000) / 71.86s (batch size = 100000)

實驗二：
分批寫入，每個batch共1萬筆，但分成n/10000個task來執行
一萬:0.205s
十萬:0.486s
百萬:4.547s
千萬:66.4s (batch size = 10000) / 42.9s (batch size = 100000)
