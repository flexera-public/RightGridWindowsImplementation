require 'yaml'
require 'rubygems'
require 'right_aws'

def upload_file(bucket, key, data)
  bucket.put(key, data)
end

def enqueue_work_unit(queue, work_unit)
  queue.send_message(work_unit)
end


# Get S3 and SQS handle
s3 = RightAws::S3Interface.new('1HF0PS1FQ264FJZ2SNG2', 'JvOS4V6xx3eUQf1zJqPt95tW2jRy4ORFQdrZeqqW')
sqs = RightAws::SqsGen2.new('1HF0PS1FQ264FJZ2SNG2', 'JvOS4V6xx3eUQf1zJqPt95tW2jRy4ORFQdrZeqqW')
inqueue = sqs.queue('rs-test', false)

# Generate work units
for id in 1...(2)
  puts "Generating work unit #{id}"

  
  work_unit = {
    :created_at => Time.now.utc.strftime('%Y-%m-%d %H:%M:%S %Z'),
    :input_file => 'input.png',
    :output_file => 'output.png',
    :id => id
  }
  s3.put('WinRightGrid', "input/input.png", IO.read("input.png"))
  wu_yaml = work_unit.to_yaml
  enqueue_work_unit(inqueue, wu_yaml)
  puts wu_yaml
end
