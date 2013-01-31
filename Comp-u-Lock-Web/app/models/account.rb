class Account < ActiveRecord::Base
  attr_accessible :computer_id, :allotted_time, :domain, :tracking, :used_time, :user_name

  validates :user_name, :presence => true
  
  belongs_to :computer

  has_many :account_history, :dependent => :destroy
  has_many :account_process, :dependent => :destroy
  has_many :account_program, :dependent => :destroy

end
