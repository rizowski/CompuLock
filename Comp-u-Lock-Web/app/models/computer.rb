class Computer < ActiveRecord::Base
  attr_accessible :enviroment, :ip_address, :name

  validates :name, :presence => true
  validates :enviroment, :presence => true
  
  has_many :account, :dependent => :destroy
end
